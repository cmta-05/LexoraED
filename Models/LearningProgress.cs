namespace LexoraED.Models;

public class LearningProgress
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }

    public Learner Learner { get; set; } = null!;
}
