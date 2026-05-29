using LexoraED.Data;
using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class AdaptiveLearningPathService
{
    private readonly LexoraEDContext _context;
    private readonly GamificationService _gamificationService;
    private readonly ActivityLogService _activityLogService;

    public AdaptiveLearningPathService(
        LexoraEDContext context,
        GamificationService gamificationService,
        ActivityLogService activityLogService)
    {
        _context = context;
        _gamificationService = gamificationService;
        _activityLogService = activityLogService;
    }

    public async Task<PerformanceEvaluationResult> EvaluateLearnerPerformanceAsync(
        string userId,
        int quizSetId,
        int score)
    {
        var quizSet = await _context.QuizSets
            .Include(q => q.LearningModule)
            .FirstOrDefaultAsync(q => q.Id == quizSetId)
            ?? throw new InvalidOperationException("Quiz set not found.");

        var attempt = new LearningAttempt
        {
            UserId = userId,
            QuizSetId = quizSetId,
            Score = score,
            AttemptDate = DateTime.UtcNow
        };
        _context.LearningAttempts.Add(attempt);

        var progress = await _context.LearningProgresses
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (progress == null)
        {
            progress = new LearningProgress
            {
                UserId = userId,
                CurrentLevel = DifficultyLevel.Beginner,
                CompletedModulesCount = 0
            };
            _context.LearningProgresses.Add(progress);
        }

        var recommendation = GetNextLearningPath(score, progress.CurrentLevel);
        var feedback = GenerateAdaptiveFeedback(score, recommendation.RecommendedLevel);

        await UpdateModuleProgressAsync(userId, quizSet.LearningModuleId, score);
        UnlockNextModule(userId, quizSet.LearningModuleId, score);

        if (score <= 50)
            progress.CurrentLevel = DifficultyLevel.Beginner;
        else if (score <= 75)
            progress.CurrentLevel = recommendation.RecommendedLevel;
        else
        {
            progress.CurrentLevel = recommendation.RecommendedLevel;
            if (score >= 76)
                progress.CompletedModulesCount++;
        }

        progress.ExperiencePoints += score >= 76 ? 100 : score >= 51 ? 50 : 25;
        progress.LastActivityDate = DateTime.UtcNow;
        progress.CurrentStreak = await CalculateStreakAsync(userId);
        progress.RecommendedModuleId = await GetRecommendedModuleIdAsync(userId, progress.CurrentLevel);

        await _context.SaveChangesAsync();

        await _gamificationService.ProcessQuizCompletionAsync(userId, score);
        await _activityLogService.LogAsync(userId, "QuizCompleted",
            $"Scored {score}% on {quizSet.LearningModule.Title}");

        return new PerformanceEvaluationResult
        {
            Score = score,
            RecommendedLevel = recommendation.RecommendedLevel,
            GuidanceMessage = feedback,
            CompletedModulesCount = progress.CompletedModulesCount,
            CurrentLevel = progress.CurrentLevel,
            RecommendedModuleId = progress.RecommendedModuleId,
            ExperiencePoints = progress.ExperiencePoints,
            CurrentStreak = progress.CurrentStreak
        };
    }

    public LearningPathRecommendation GetNextLearningPath(int score, DifficultyLevel currentLevel)
    {
        if (score <= 50)
        {
            return new LearningPathRecommendation
            {
                RecommendedLevel = DifficultyLevel.Beginner,
                GuidanceMessage = GenerateAdaptiveFeedback(score, DifficultyLevel.Beginner)
            };
        }

        if (score <= 75)
        {
            return new LearningPathRecommendation
            {
                RecommendedLevel = currentLevel,
                GuidanceMessage = GenerateAdaptiveFeedback(score, currentLevel)
            };
        }

        var nextLevel = currentLevel switch
        {
            DifficultyLevel.Beginner => DifficultyLevel.Intermediate,
            DifficultyLevel.Intermediate => DifficultyLevel.Advanced,
            _ => DifficultyLevel.Advanced
        };

        return new LearningPathRecommendation
        {
            RecommendedLevel = nextLevel,
            GuidanceMessage = GenerateAdaptiveFeedback(score, nextLevel)
        };
    }

    public string GenerateAdaptiveFeedback(int score, DifficultyLevel recommendedLevel)
    {
        if (score <= 50)
            return "You should review beginner lessons first. Focus on foundational vocabulary and greetings before advancing.";

        if (score <= 75)
            return "You are progressing well. Continue practicing at your current level to strengthen understanding.";

        return recommendedLevel switch
        {
            DifficultyLevel.Intermediate =>
                "Excellent mastery. Intermediate lessons are now recommended for your learning path.",
            DifficultyLevel.Advanced =>
                "Excellent mastery. Advanced lessons unlocked — challenge yourself with complex English skills.",
            _ => "Excellent mastery. Keep building confidence with beginner microlessons."
        };
    }

    public async Task UnlockNextModuleAsync(string userId, int completedModuleId, int score)
    {
        UnlockNextModule(userId, completedModuleId, score);
        await _context.SaveChangesAsync();
    }

    private void UnlockNextModule(string userId, int completedModuleId, int score)
    {
        if (score < 76)
            return;

        var completed = _context.LearningModules.AsNoTracking()
            .FirstOrDefault(m => m.Id == completedModuleId);
        if (completed == null)
            return;

        var next = _context.LearningModules
            .Where(m => m.SortOrder == completed.SortOrder + 1 &&
                        m.DifficultyLevel == completed.DifficultyLevel)
            .FirstOrDefault();

        if (next == null)
            return;

        var mp = _context.ModuleProgresses
            .FirstOrDefault(p => p.UserId == userId && p.LearningModuleId == next.Id);

        if (mp == null)
        {
            _context.ModuleProgresses.Add(new ModuleProgress
            {
                UserId = userId,
                LearningModuleId = next.Id,
                LastAccessedAt = DateTime.UtcNow
            });
        }
    }

    private async Task UpdateModuleProgressAsync(string userId, int moduleId, int score)
    {
        var mp = await _context.ModuleProgresses
            .FirstOrDefaultAsync(p => p.UserId == userId && p.LearningModuleId == moduleId);

        if (mp == null)
        {
            mp = new ModuleProgress { UserId = userId, LearningModuleId = moduleId };
            _context.ModuleProgresses.Add(mp);
        }

        mp.BestScore = Math.Max(mp.BestScore, score);
        mp.LastAccessedAt = DateTime.UtcNow;
        if (score >= 76)
        {
            mp.IsCompleted = true;
            mp.CompletedAt = DateTime.UtcNow;
        }
    }

    private async Task<int?> GetRecommendedModuleIdAsync(string userId, DifficultyLevel level)
    {
        var completedIds = await _context.ModuleProgresses
            .Where(p => p.UserId == userId && p.IsCompleted)
            .Select(p => p.LearningModuleId)
            .ToListAsync();

        return await _context.LearningModules
            .Where(m => m.DifficultyLevel == level && !completedIds.Contains(m.Id))
            .OrderBy(m => m.SortOrder)
            .Select(m => (int?)m.Id)
            .FirstOrDefaultAsync();
    }

    private async Task<int> CalculateStreakAsync(string userId)
    {
        var dates = await _context.LearningAttempts
            .Where(a => a.UserId == userId)
            .Select(a => a.AttemptDate.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .Take(30)
            .ToListAsync();

        if (!dates.Any())
            return 0;

        var streak = 1;
        for (var i = 1; i < dates.Count; i++)
        {
            if (dates[i] == dates[i - 1].AddDays(-1))
                streak++;
            else
                break;
        }

        return streak;
    }

    public async Task<List<LearningModule>> GetRecommendedModulesAsync(string userId)
    {
        var progress = await _context.LearningProgresses
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);

        var targetLevel = progress?.CurrentLevel ?? DifficultyLevel.Beginner;

        return await _context.LearningModules
            .AsNoTracking()
            .Where(m => m.DifficultyLevel == targetLevel)
            .OrderBy(m => m.SortOrder)
            .ThenBy(m => m.Category)
            .ToListAsync();
    }
}

public class PerformanceEvaluationResult
{
    public int Score { get; set; }
    public DifficultyLevel RecommendedLevel { get; set; }
    public string GuidanceMessage { get; set; } = string.Empty;
    public int CompletedModulesCount { get; set; }
    public DifficultyLevel CurrentLevel { get; set; }
    public int? RecommendedModuleId { get; set; }
    public int ExperiencePoints { get; set; }
    public int CurrentStreak { get; set; }
}

public class LearningPathRecommendation
{
    public DifficultyLevel RecommendedLevel { get; set; }
    public string GuidanceMessage { get; set; } = string.Empty;
}
