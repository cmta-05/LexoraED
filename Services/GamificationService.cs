using LexoraED.Data;
using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class GamificationService
{
    private readonly LexoraEDContext _context;

    public GamificationService(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task ProcessQuizCompletionAsync(int learnerId, int score)
    {
        await EnsureBadgesExistAsync();

        if (score == 100)
            await AwardBadgeAsync(learnerId, "PERFECT_SCORE");

        if (score >= 76)
            await AwardBadgeAsync(learnerId, "MODULE_MASTER");

        var attemptCount = await _context.LearningAttempts.CountAsync(a => a.LearnerId == learnerId);
        if (attemptCount >= 5)
            await AwardBadgeAsync(learnerId, "QUIZ_STREAK_5");

        var grammarAttempts = await _context.LearningAttempts
            .Include(a => a.QuizSet).ThenInclude(q => q.LearningModule)
            .CountAsync(a => a.LearnerId == learnerId &&
                             a.QuizSet.LearningModule.Category == ModuleCategory.Grammar &&
                             a.Score >= 76);

        if (grammarAttempts >= 2)
            await AwardBadgeAsync(learnerId, "GRAMMAR_BEGINNER");
    }

    public async Task<List<LearnerAchievement>> GetLearnerBadgesAsync(int learnerId)
    {
        return await _context.LearnerAchievements
            .AsNoTracking()
            .Include(a => a.AchievementBadge)
            .Where(a => a.LearnerId == learnerId)
            .OrderByDescending(a => a.EarnedAt)
            .ToListAsync();
    }

    private async Task AwardBadgeAsync(int learnerId, string code)
    {
        var badge = await _context.AchievementBadges.FirstOrDefaultAsync(b => b.Code == code);
        if (badge == null)
            return;

        var exists = await _context.LearnerAchievements
            .AnyAsync(a => a.LearnerId == learnerId && a.AchievementBadgeId == badge.Id);

        if (exists)
            return;

        _context.LearnerAchievements.Add(new LearnerAchievement
        {
            LearnerId = learnerId,
            AchievementBadgeId = badge.Id,
            EarnedAt = DateTime.UtcNow
        });

        var progress = await _context.LearningProgresses.FirstOrDefaultAsync(p => p.LearnerId == learnerId);
        if (progress != null)
            progress.ExperiencePoints += badge.PointsAwarded;

        await _context.SaveChangesAsync();
    }

    private async Task EnsureBadgesExistAsync()
    {
        if (await _context.AchievementBadges.AnyAsync())
            return;

        _context.AchievementBadges.AddRange(
            new AchievementBadge { Code = "GRAMMAR_BEGINNER", Title = "Grammar Beginner", Description = "Completed grammar lessons with strong scores.", PointsAwarded = 50, IconClass = "badge-grammar" },
            new AchievementBadge { Code = "VOCAB_MASTER", Title = "Vocabulary Master", Description = "Excelled in vocabulary modules.", PointsAwarded = 75, IconClass = "badge-vocab" },
            new AchievementBadge { Code = "QUIZ_STREAK_5", Title = "Quiz Streak x5", Description = "Completed five quiz attempts.", PointsAwarded = 40, IconClass = "badge-streak" },
            new AchievementBadge { Code = "PERFECT_SCORE", Title = "Perfect Score Award", Description = "Achieved 100% on a quiz.", PointsAwarded = 100, IconClass = "badge-perfect" },
            new AchievementBadge { Code = "MODULE_MASTER", Title = "Module Master", Description = "Mastered a module with 76% or higher.", PointsAwarded = 60, IconClass = "badge-master" });

        await _context.SaveChangesAsync();
    }
}
