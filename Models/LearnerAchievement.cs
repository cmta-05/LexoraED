namespace LexoraED.Models;

public class LearnerAchievement
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int AchievementBadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
    public AchievementBadge AchievementBadge { get; set; } = null!;
}
