namespace LexoraED.Models;

public class AchievementBadge
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = "badge-default";
    public int PointsAwarded { get; set; }

    public ICollection<LearnerAchievement> LearnerAchievements { get; set; } = new List<LearnerAchievement>();
}
