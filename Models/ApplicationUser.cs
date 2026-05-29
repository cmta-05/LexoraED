using Microsoft.AspNetCore.Identity;

namespace LexoraED.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Approved;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public StudentProfile? StudentProfile { get; set; }
    public TeacherProfile? TeacherProfile { get; set; }
    public LearningProgress? LearningProgress { get; set; }
    public ICollection<LearningAttempt> LearningAttempts { get; set; } = new List<LearningAttempt>();
    public ICollection<ModuleProgress> ModuleProgresses { get; set; } = new List<ModuleProgress>();
    public ICollection<LearnerAchievement> LearnerAchievements { get; set; } = new List<LearnerAchievement>();
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
