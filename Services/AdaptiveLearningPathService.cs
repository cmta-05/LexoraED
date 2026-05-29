using LexoraED.Data;
using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class AdaptiveLearningPathService
{
    private readonly LexoraEDContext _context;

    public AdaptiveLearningPathService(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task<PerformanceEvaluationResult> EvaluateLearnerPerformanceAsync(
        int learnerId,
        int quizSetId,
        int score)
    {
        var attempt = new LearningAttempt
        {
            LearnerId = learnerId,
            QuizSetId = quizSetId,
            Score = score,
            AttemptDate = DateTime.UtcNow
        };

        _context.LearningAttempts.Add(attempt);

        var progress = await _context.LearningProgresses
            .FirstOrDefaultAsync(p => p.LearnerId == learnerId);

        if (progress == null)
        {
            progress = new LearningProgress
            {
                LearnerId = learnerId,
                CurrentLevel = DifficultyLevel.Easy,
                CompletedModulesCount = 0
            };
            _context.LearningProgresses.Add(progress);
        }

        var recommendedPath = GetNextLearningPath(score, progress.CurrentLevel);

        if (score >= 80)
        {
            progress.CompletedModulesCount++;
            progress.CurrentLevel = recommendedPath.RecommendedLevel;
        }
        else if (score >= 50)
        {
            progress.CurrentLevel = recommendedPath.RecommendedLevel;
        }
        else
        {
            progress.CurrentLevel = DifficultyLevel.Easy;
        }

        await _context.SaveChangesAsync();

        return new PerformanceEvaluationResult
        {
            Score = score,
            RecommendedLevel = recommendedPath.RecommendedLevel,
            GuidanceMessage = recommendedPath.GuidanceMessage,
            CompletedModulesCount = progress.CompletedModulesCount,
            CurrentLevel = progress.CurrentLevel
        };
    }

    public LearningPathRecommendation GetNextLearningPath(int score, DifficultyLevel currentLevel)
    {
        if (score < 50)
        {
            return new LearningPathRecommendation
            {
                RecommendedLevel = DifficultyLevel.Easy,
                GuidanceMessage = "Your score suggests reviewing foundational material. We recommend Easy-level modules to strengthen your understanding."
            };
        }

        if (score <= 80)
        {
            return new LearningPathRecommendation
            {
                RecommendedLevel = currentLevel,
                GuidanceMessage = "Solid progress. Continue practicing at your current difficulty level to build confidence."
            };
        }

        var nextLevel = currentLevel switch
        {
            DifficultyLevel.Easy => DifficultyLevel.Medium,
            DifficultyLevel.Medium => DifficultyLevel.Hard,
            _ => DifficultyLevel.Hard
        };

        return new LearningPathRecommendation
        {
            RecommendedLevel = nextLevel,
            GuidanceMessage = "Excellent work! You have unlocked the next learning level. Challenge yourself with more advanced modules."
        };
    }

    public async Task<List<LearningModule>> GetRecommendedModulesAsync(int learnerId)
    {
        var progress = await _context.LearningProgresses
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.LearnerId == learnerId);

        var targetLevel = progress?.CurrentLevel ?? DifficultyLevel.Easy;

        return await _context.LearningModules
            .AsNoTracking()
            .Where(m => m.DifficultyLevel == targetLevel)
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Title)
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
}

public class LearningPathRecommendation
{
    public DifficultyLevel RecommendedLevel { get; set; }
    public string GuidanceMessage { get; set; } = string.Empty;
}
