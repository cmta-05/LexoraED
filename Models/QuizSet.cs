namespace LexoraED.Models;

public class QuizSet
{
    public int Id { get; set; }
    public int LearningModuleId { get; set; }

    public LearningModule LearningModule { get; set; } = null!;
    public ICollection<QuizItem> QuizItems { get; set; } = new List<QuizItem>();
    public ICollection<LearningAttempt> LearningAttempts { get; set; } = new List<LearningAttempt>();
}
