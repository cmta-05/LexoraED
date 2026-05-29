using LexoraED.Models;

namespace LexoraED.ViewModels;

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
    public string StudentId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DifficultyLevel CurrentLevel { get; set; }
    public double AverageScore { get; set; }
    public int CompletedModules { get; set; }
    public DifficultyLevel RecommendedLevel { get; set; }
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
