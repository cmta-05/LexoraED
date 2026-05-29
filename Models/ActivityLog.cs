namespace LexoraED.Models;

public class ActivityLog
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;

    public Learner Learner { get; set; } = null!;
}
