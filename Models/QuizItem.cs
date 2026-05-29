namespace LexoraED.Models;

public class QuizItem
{
    public int Id { get; set; }
    public int QuizSetId { get; set; }
    public QuizQuestionType QuestionType { get; set; } = QuizQuestionType.MultipleChoice;
    public string QuestionText { get; set; } = string.Empty;
    public string ChoiceA { get; set; } = string.Empty;
    public string ChoiceB { get; set; } = string.Empty;
    public string ChoiceC { get; set; } = string.Empty;
    public string ChoiceD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string? Explanation { get; set; }

    public QuizSet QuizSet { get; set; } = null!;
}
