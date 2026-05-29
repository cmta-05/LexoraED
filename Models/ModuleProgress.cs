namespace LexoraED.Models;

public class ModuleProgress
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public int LearningModuleId { get; set; }
    public bool IsCompleted { get; set; }
    public int BestScore { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;

    public Learner Learner { get; set; } = null!;
    public LearningModule LearningModule { get; set; } = null!;
}
