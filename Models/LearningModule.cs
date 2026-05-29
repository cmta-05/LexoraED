namespace LexoraED.Models;

public class LearningModule
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ModuleCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }

    public ICollection<QuizSet> QuizSets { get; set; } = new List<QuizSet>();
}
