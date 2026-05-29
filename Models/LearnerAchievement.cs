namespace LexoraED.Models;

public class LearnerAchievement
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public int AchievementBadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

    public Learner Learner { get; set; } = null!;
    public AchievementBadge AchievementBadge { get; set; } = null!;
}
