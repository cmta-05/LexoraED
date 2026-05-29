using System.ComponentModel.DataAnnotations;
using LexoraED.Models;

namespace LexoraED.ViewModels;

public class LearnerManagementIndexViewModel
{
    public List<LearnerCardViewModel> Learners { get; set; } = new();
    public string? Search { get; set; }
    public LearnerRole? RoleFilter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

public class LearnerCardViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public LearnerRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LearnerFormViewModel
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string? Password { get; set; }

    public LearnerRole Role { get; set; } = LearnerRole.Student;
    public bool IsActive { get; set; } = true;
}

public class LearnerDetailsViewModel
{
    public LearnerCardViewModel Learner { get; set; } = new();
    public LearningProgress? Progress { get; set; }
    public List<LearnerAchievement> Achievements { get; set; } = new();
}

public class TeacherDashboardViewModel
{
    public int TotalStudents { get; set; }
    public double AverageStudentScore { get; set; }
    public string MostDifficultCategory { get; set; } = string.Empty;
    public double LearningCompletionRate { get; set; }
    public List<TeacherActivityItemViewModel> RecentActivity { get; set; } = new();
    public List<ChartDataPointViewModel> CategoryChartData { get; set; } = new();
    public List<ChartDataPointViewModel> ScoreTrendData { get; set; } = new();
}

public class TeacherActivityItemViewModel
{
    public string StudentName { get; set; } = string.Empty;
    public string ModuleTitle { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime AttemptDate { get; set; }
}

public class ChartDataPointViewModel
{
    public string Label { get; set; } = string.Empty;
    public double Value { get; set; }
}

public class TeacherAnalyticsViewModel
{
    public TeacherDashboardViewModel Dashboard { get; set; } = new();
    public List<TeacherStudentSummaryViewModel> StudentSummaries { get; set; } = new();
    public List<CategoryWeaknessViewModel> WeakCategories { get; set; } = new();
}

public class TeacherStudentSummaryViewModel
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DifficultyLevel CurrentLevel { get; set; }
    public DifficultyLevel RecommendedLevel { get; set; }
    public double AverageScore { get; set; }
    public int CompletedModules { get; set; }
}

public class StudentReportViewModel
{
    public string LearnerName { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public StudentProgressAnalyticsViewModel Analytics { get; set; } = new();
    public List<RecentAttemptViewModel> QuizAttempts { get; set; } = new();
    public List<string> Badges { get; set; } = new();
}

public class TeacherReportViewModel
{
    public DateTime GeneratedAt { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TeacherAnalyticsViewModel Analytics { get; set; } = new();
}

public class AdminReportViewModel
{
    public DateTime GeneratedAt { get; set; }
    public int TotalUsers { get; set; }
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalAdmins { get; set; }
    public int TotalModules { get; set; }
    public int TotalQuizAttempts { get; set; }
    public double PlatformAverageScore { get; set; }
    public int ActiveUsers { get; set; }
}

public class QuizItemFormViewModel
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
}
