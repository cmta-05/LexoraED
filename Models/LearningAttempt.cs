namespace LexoraED.Models;

public class LearningAttempt
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int QuizSetId { get; set; }
    public int Score { get; set; }
    public DateTime AttemptDate { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public QuizSet QuizSet { get; set; } = null!;
}
