namespace LexoraED.Models;

public class ModuleProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int LearningModuleId { get; set; }
    public bool IsCompleted { get; set; }
    public int BestScore { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
    public LearningModule LearningModule { get; set; } = null!;
}
