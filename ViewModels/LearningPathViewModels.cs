using LexoraED.Models;

namespace LexoraED.ViewModels;

public class LearningPathDashboardViewModel
{
    public string LearnerName { get; set; } = string.Empty;
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }
    public double AverageScore { get; set; }
    public List<DifficultyTierViewModel> DifficultyTiers { get; set; } = new();
    public PathProgressionViewModel PathProgression { get; set; } = new();
}

public class DifficultyTierViewModel
{
    public DifficultyLevel Level { get; set; }
    public bool IsUnlocked { get; set; }
    public bool IsCurrentTier { get; set; }
    public List<LearningModuleCardViewModel> Modules { get; set; } = new();
}

public class LearningModuleCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ModuleCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsRecommended { get; set; }
    public int ProgressPercent { get; set; }
    public bool HasQuiz { get; set; }
}

public class PathProgressionViewModel
{
    public DifficultyLevel CurrentLevel { get; set; }
    public DifficultyLevel RecommendedLevel { get; set; }
    public bool EasyUnlocked { get; set; } = true;
    public bool MediumUnlocked { get; set; }
    public bool HardUnlocked { get; set; }
    public int? RecommendedModuleId { get; set; }
    public string? RecommendedModuleTitle { get; set; }
}

public class LearningModulesPageViewModel
{
    public List<DifficultyTierViewModel> DifficultyTiers { get; set; } = new();
    public PathProgressionViewModel PathProgression { get; set; } = new();
}

public class StudyModuleViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ModuleCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public bool HasQuiz { get; set; }
    public bool IsLocked { get; set; }
}

public class QuizCenterViewModel
{
    public List<QuizCenterItemViewModel> AvailableQuizzes { get; set; } = new();
}

public class QuizCenterItemViewModel
{
    public int ModuleId { get; set; }
    public string ModuleTitle { get; set; } = string.Empty;
    public ModuleCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int QuestionCount { get; set; }
    public int? BestScore { get; set; }
    public bool IsLocked { get; set; }
}

public class StudentProgressAnalyticsViewModel
{
    public string LearnerName { get; set; } = string.Empty;
    public double AverageScore { get; set; }
    public int CompletedModulesCount { get; set; }
    public DifficultyLevel CurrentLevel { get; set; }
    public List<CategoryProgressViewModel> CategoryProgress { get; set; } = new();
    public List<RecentAttemptViewModel> RecentAttempts { get; set; } = new();
    public PathProgressionViewModel PathProgression { get; set; } = new();
}

public class CategoryProgressViewModel
{
    public ModuleCategory Category { get; set; }
    public int TotalModules { get; set; }
    public int CompletedModules { get; set; }
    public int ProgressPercent { get; set; }
    public double AverageScore { get; set; }
}

public class RecentAttemptViewModel
{
    public string ModuleTitle { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime AttemptDate { get; set; }
}

public class AdminProgressAnalyticsViewModel
{
    public double OverallAverageScore { get; set; }
    public int TotalLearners { get; set; }
    public List<AdminLearnerSummaryViewModel> Learners { get; set; } = new();
    public List<CategoryWeaknessViewModel> CategoryOverview { get; set; } = new();
}

public class AdminLearnerSummaryViewModel
{
    public int LearnerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }
    public double AverageScore { get; set; }
}

public class CategoryWeaknessViewModel
{
    public ModuleCategory Category { get; set; }
    public double AverageScore { get; set; }
    public int AttemptCount { get; set; }
    public bool IsWeakArea { get; set; }
}
