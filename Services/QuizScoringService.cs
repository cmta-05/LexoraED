using LexoraED.Models;

namespace LexoraED.Services;

public class QuizScoringService
{
    public bool IsAnswerCorrect(QuizItem item, string? selectedAnswer)
    {
        if (string.IsNullOrWhiteSpace(selectedAnswer) || string.IsNullOrWhiteSpace(item.CorrectAnswer))
            return false;

        return item.QuestionType switch
        {
            QuizQuestionType.MultipleChoice => string.Equals(
                selectedAnswer.Trim(), item.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase),
            QuizQuestionType.TrueFalse => string.Equals(
                selectedAnswer.Trim(), item.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase),
            QuizQuestionType.Identification => string.Equals(
                NormalizeIdentification(selectedAnswer),
                NormalizeIdentification(item.CorrectAnswer),
                StringComparison.OrdinalIgnoreCase),
            QuizQuestionType.Situational => GradeSituational(selectedAnswer, item.CorrectAnswer),
            _ => false
        };
    }

    private static string NormalizeIdentification(string value)
        => value.Trim().TrimEnd('.', '!', '?');

    private static bool GradeSituational(string answer, string correct)
    {
        var normalized = answer.Trim().ToLowerInvariant();
        var keywords = correct.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (keywords.Length == 0)
            return normalized.Contains(correct.Trim().ToLowerInvariant());

        return keywords.Any(k => normalized.Contains(k.ToLowerInvariant()));
    }
}
