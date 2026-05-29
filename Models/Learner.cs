namespace LexoraED.Models;

public class Learner
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public LearnerRole Role { get; set; }

    public ICollection<LearningAttempt> LearningAttempts { get; set; } = new List<LearningAttempt>();
    public LearningProgress? LearningProgress { get; set; }
}
