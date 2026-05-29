using LexoraED.Models;

namespace LexoraED.ViewModels;

public class LearningPathDashboardViewModel
{
    public string LearnerName { get; set; } = string.Empty;
    public DifficultyLevel CurrentLevel { get; set; }
    public int CompletedModulesCount { get; set; }
    public double AverageScore { get; set; }
    public int ExperiencePoints { get; set; }
    public int CurrentStreak { get; set; }
    public int OverallCompletionPercent { get; set; }
    public List<DifficultyTierViewModel> DifficultyTiers { get; set; } = new();
    public PathProgressionViewModel PathProgression { get; set; } = new();
    public LearningPathMapViewModel PathMap { get; set; } = new();
    public List<LearnerBadgeViewModel> Badges { get; set; } = new();
    public LearningModuleCardViewModel? RecommendedModule { get; set; }
}

public class LearningPathMapViewModel
{
    public List<PathMapNodeViewModel> Nodes { get; set; } = new();
}

public class PathMapNodeViewModel
{
    public int ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Level { get; set; }
    public PathMapNodeState State { get; set; }
    public int SortOrder { get; set; }
}

public enum PathMapNodeState
{
    Locked,
    Current,
    Completed,
    Recommended
}

public class LearnerBadgeViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EarnedAt { get; set; }
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
    public bool BeginnerUnlocked { get; set; } = true;
    public bool IntermediateUnlocked { get; set; }
    public bool AdvancedUnlocked { get; set; }
    public int? RecommendedModuleId { get; set; }
    public string? RecommendedModuleTitle { get; set; }
}

public class LearningModulesPageViewModel
{
    public List<DifficultyTierViewModel> DifficultyTiers { get; set; } = new();
    public PathProgressionViewModel PathProgression { get; set; } = new();
    public string? Search { get; set; }
    public ModuleCategory? CategoryFilter { get; set; }
    public DifficultyLevel? DifficultyFilter { get; set; }
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
