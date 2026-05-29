namespace LexoraED.Models;

public class LearningAttempt
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public int QuizSetId { get; set; }
    public int Score { get; set; }
    public DateTime AttemptDate { get; set; }

    public Learner Learner { get; set; } = null!;
    public QuizSet QuizSet { get; set; } = null!;
}
