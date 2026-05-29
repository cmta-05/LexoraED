namespace LexoraED.Models;

public class LearningProgress
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }
    public int ExperiencePoints { get; set; }
    public int CurrentStreak { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public int? RecommendedModuleId { get; set; }

    public Learner Learner { get; set; } = null!;
}
