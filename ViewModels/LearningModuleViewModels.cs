using System.ComponentModel.DataAnnotations;
using LexoraED.Models;

namespace LexoraED.ViewModels;

public class LearningModuleFormViewModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public ModuleCategory Category { get; set; }

    public DifficultyLevel DifficultyLevel { get; set; }
}

public class QuizSubmissionViewModel
{
    public int QuizSetId { get; set; }
    public int ModuleId { get; set; }
    public string ModuleTitle { get; set; } = string.Empty;
    public List<QuizAnswerViewModel> Answers { get; set; } = new();
}

public class QuizAnswerViewModel
{
    public int QuizItemId { get; set; }
    public QuizQuestionType QuestionType { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string ChoiceA { get; set; } = string.Empty;
    public string ChoiceB { get; set; } = string.Empty;
    public string ChoiceC { get; set; } = string.Empty;
    public string ChoiceD { get; set; } = string.Empty;
    public string? SelectedAnswer { get; set; }
}

public class QuizResultViewModel
{
    public int Score { get; set; }
    public string GuidanceMessage { get; set; } = string.Empty;
    public string FeedbackTier { get; set; } = string.Empty;
    public string FeedbackHeadline { get; set; } = string.Empty;
    public DifficultyLevel RecommendedLevel { get; set; }
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }
    public int ExperiencePoints { get; set; }
    public int CurrentStreak { get; set; }
    public int ModuleId { get; set; }
    public string ModuleTitle { get; set; } = string.Empty;
    public List<QuizAnswerReviewViewModel> AnswerReviews { get; set; } = new();
}

public static class QuizFeedbackHelper
{
    public static (string Tier, string Headline) GetFeedback(int score) => score switch
    {
        >= 76 => ("Excellent mastery", "Outstanding work!"),
        >= 51 => ("Good progress", "You're on the right track."),
        _ => ("Needs improvement", "Keep practicing — you've got this.")
    };
}

public class QuizAnswerReviewViewModel
{
    public string QuestionText { get; set; } = string.Empty;
    public string? SelectedAnswer { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public string? Explanation { get; set; }
    public QuizQuestionType QuestionType { get; set; }
}

public class QuizItemFormViewModel
{
    public int QuizSetId { get; set; }

    [Required]
    public QuizQuestionType QuestionType { get; set; }

    [Required]
    public string QuestionText { get; set; } = string.Empty;

    public string? ChoiceA { get; set; }
    public string? ChoiceB { get; set; }
    public string? ChoiceC { get; set; }
    public string? ChoiceD { get; set; }

    [Required]
    public string CorrectAnswer { get; set; } = string.Empty;

    public string? Explanation { get; set; }
}

public class StudentDashboardViewModel
{
    public string LearnerName { get; set; } = string.Empty;
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }
    public List<LearningModule> RecommendedModules { get; set; } = new();
    public List<LearningAttempt> RecentAttempts { get; set; } = new();
}
