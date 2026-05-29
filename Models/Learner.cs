namespace LexoraED.Models;

public class Learner
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public LearnerRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TeacherProfile? TeacherProfile { get; set; }
    public ICollection<LearningAttempt> LearningAttempts { get; set; } = new List<LearningAttempt>();
    public LearningProgress? LearningProgress { get; set; }
    public ICollection<ModuleProgress> ModuleProgresses { get; set; } = new List<ModuleProgress>();
    public ICollection<LearnerAchievement> LearnerAchievements { get; set; } = new List<LearnerAchievement>();
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
