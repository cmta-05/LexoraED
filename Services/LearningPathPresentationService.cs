using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class LearningPathPresentationService
{
    private readonly LexoraEDContext _context;

    public LearningPathPresentationService(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task<LearningPathDashboardViewModel> BuildDashboardAsync(int learnerId, string learnerName)
    {
        var pathData = await BuildPathDataAsync(learnerId);

        return new LearningPathDashboardViewModel
        {
            LearnerName = learnerName,
            CurrentLevel = pathData.CurrentLevel,
            CompletedModulesCount = pathData.CompletedModulesCount,
            AverageScore = pathData.AverageScore,
            DifficultyTiers = pathData.Tiers,
            PathProgression = pathData.Progression
        };
    }

    public async Task<LearningModulesPageViewModel> BuildModulesPageAsync(int learnerId)
    {
        var pathData = await BuildPathDataAsync(learnerId);
        return new LearningModulesPageViewModel
        {
            DifficultyTiers = pathData.Tiers,
            PathProgression = pathData.Progression
        };
    }

    public async Task<StudyModuleViewModel?> BuildStudyModuleAsync(int learnerId, int moduleId)
    {
        var module = await _context.LearningModules
            .AsNoTracking()
            .Include(m => m.QuizSets)
                .ThenInclude(q => q.QuizItems)
            .FirstOrDefaultAsync(m => m.Id == moduleId);

        if (module == null)
            return null;

        var pathData = await BuildPathDataAsync(learnerId);
        var card = pathData.Tiers
            .SelectMany(t => t.Modules)
            .FirstOrDefault(m => m.Id == moduleId);

        return new StudyModuleViewModel
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Content = module.Content,
            Category = module.Category,
            DifficultyLevel = module.DifficultyLevel,
            HasQuiz = module.QuizSets.Any(q => q.QuizItems.Any()),
            IsLocked = card?.IsLocked ?? IsModuleLocked(module.DifficultyLevel, pathData.CurrentLevel)
        };
    }

    public async Task<QuizCenterViewModel> BuildQuizCenterAsync(int learnerId)
    {
        var pathData = await BuildPathDataAsync(learnerId);
        var modules = pathData.Tiers.SelectMany(t => t.Modules).Where(m => m.HasQuiz).ToList();

        var quizSets = await _context.QuizSets
            .AsNoTracking()
            .Include(q => q.QuizItems)
            .Where(q => modules.Select(m => m.Id).Contains(q.LearningModuleId))
            .ToListAsync();

        var attempts = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet)
            .Where(a => a.LearnerId == learnerId)
            .ToListAsync();

        var items = modules.Select(m =>
        {
            var qs = quizSets.FirstOrDefault(q => q.LearningModuleId == m.Id);
            var best = attempts
                .Where(a => a.QuizSet.LearningModuleId == m.Id)
                .Select(a => (int?)a.Score)
                .DefaultIfEmpty()
                .Max();

            return new QuizCenterItemViewModel
            {
                ModuleId = m.Id,
                ModuleTitle = m.Title,
                Category = m.Category,
                DifficultyLevel = m.DifficultyLevel,
                QuestionCount = qs?.QuizItems.Count ?? 0,
                BestScore = best,
                IsLocked = m.IsLocked
            };
        }).ToList();

        return new QuizCenterViewModel { AvailableQuizzes = items };
    }

    public async Task<StudentProgressAnalyticsViewModel> BuildStudentAnalyticsAsync(int learnerId, string learnerName)
    {
        var pathData = await BuildPathDataAsync(learnerId);

        var attempts = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet)
                .ThenInclude(q => q.LearningModule)
            .Where(a => a.LearnerId == learnerId)
            .OrderByDescending(a => a.AttemptDate)
            .ToListAsync();

        var allModules = await _context.LearningModules.AsNoTracking().ToListAsync();
        var completedModuleIds = await GetCompletedModuleIdsAsync(learnerId);

        var categoryProgress = Enum.GetValues<ModuleCategory>()
            .Select(category =>
            {
                var inCategory = allModules.Where(m => m.Category == category).ToList();
                var completed = inCategory.Count(m => completedModuleIds.Contains(m.Id));
                var categoryAttempts = attempts
                    .Where(a => a.QuizSet.LearningModule.Category == category)
                    .ToList();

                return new CategoryProgressViewModel
                {
                    Category = category,
                    TotalModules = inCategory.Count,
                    CompletedModules = completed,
                    ProgressPercent = inCategory.Count == 0
                        ? 0
                        : (int)Math.Round((double)completed / inCategory.Count * 100),
                    AverageScore = categoryAttempts.Count == 0
                        ? 0
                        : Math.Round(categoryAttempts.Average(a => a.Score), 1)
                };
            })
            .ToList();

        return new StudentProgressAnalyticsViewModel
        {
            LearnerName = learnerName,
            AverageScore = pathData.AverageScore,
            CompletedModulesCount = pathData.CompletedModulesCount,
            CurrentLevel = pathData.CurrentLevel,
            CategoryProgress = categoryProgress,
            RecentAttempts = attempts.Take(8).Select(a => new RecentAttemptViewModel
            {
                ModuleTitle = a.QuizSet.LearningModule.Title,
                Score = a.Score,
                AttemptDate = a.AttemptDate
            }).ToList(),
            PathProgression = pathData.Progression
        };
    }

    public async Task<AdminProgressAnalyticsViewModel> BuildAdminAnalyticsAsync()
    {
        var students = await _context.Learners
            .AsNoTracking()
            .Where(l => l.Role == LearnerRole.Student)
            .Include(l => l.LearningProgress)
            .ToListAsync();

        var attempts = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet)
                .ThenInclude(q => q.LearningModule)
            .ToListAsync();

        var learnerSummaries = students.Select(s =>
        {
            var learnerAttempts = attempts.Where(a => a.LearnerId == s.Id).ToList();
            return new AdminLearnerSummaryViewModel
            {
                LearnerId = s.Id,
                FullName = s.FullName,
                Username = s.Username,
                CurrentLevel = s.LearningProgress?.CurrentLevel ?? DifficultyLevel.Easy,
                CompletedModulesCount = s.LearningProgress?.CompletedModulesCount ?? 0,
                AverageScore = learnerAttempts.Count == 0
                    ? 0
                    : Math.Round(learnerAttempts.Average(a => a.Score), 1)
            };
        }).OrderByDescending(l => l.AverageScore).ToList();

        var categoryOverview = Enum.GetValues<ModuleCategory>()
            .Select(category =>
            {
                var categoryAttempts = attempts
                    .Where(a => a.QuizSet.LearningModule.Category == category)
                    .ToList();
                var avg = categoryAttempts.Count == 0
                    ? 0
                    : Math.Round(categoryAttempts.Average(a => a.Score), 1);

                return new CategoryWeaknessViewModel
                {
                    Category = category,
                    AverageScore = avg,
                    AttemptCount = categoryAttempts.Count,
                    IsWeakArea = false
                };
            })
            .ToList();

        var weakestThreshold = categoryOverview.Where(c => c.AttemptCount > 0).Select(c => c.AverageScore).DefaultIfEmpty(100).Min();
        foreach (var item in categoryOverview.Where(c => c.AttemptCount > 0))
        {
            item.IsWeakArea = item.AverageScore <= weakestThreshold + 5;
        }

        return new AdminProgressAnalyticsViewModel
        {
            OverallAverageScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(a => a.Score), 1),
            TotalLearners = students.Count,
            Learners = learnerSummaries,
            CategoryOverview = categoryOverview
        };
    }

    private async Task<PathBuildResult> BuildPathDataAsync(int learnerId)
    {
        var progress = await _context.LearningProgresses
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.LearnerId == learnerId);

        var currentLevel = progress?.CurrentLevel ?? DifficultyLevel.Easy;
        var completedCount = progress?.CompletedModulesCount ?? 0;

        var modules = await _context.LearningModules
            .AsNoTracking()
            .Include(m => m.QuizSets)
                .ThenInclude(q => q.QuizItems)
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Title)
            .ToListAsync();

        var attempts = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet)
            .Where(a => a.LearnerId == learnerId)
            .ToListAsync();

        var completedModuleIds = await GetCompletedModuleIdsAsync(learnerId);
        var averageScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(a => a.Score), 1);

        var moduleScores = modules.ToDictionary(
            m => m.Id,
            m =>
            {
                var moduleAttempts = attempts.Where(a => a.QuizSet.LearningModuleId == m.Id).ToList();
                return moduleAttempts.Count == 0 ? 0 : moduleAttempts.Max(a => a.Score);
            });

        var tiers = new List<DifficultyTierViewModel>();
        LearningModuleCardViewModel? recommendedCard = null;

        foreach (var level in new[] { DifficultyLevel.Easy, DifficultyLevel.Medium, DifficultyLevel.Hard })
        {
            var tierUnlocked = !IsModuleLocked(level, currentLevel);
            var tierModules = modules.Where(m => m.DifficultyLevel == level).ToList();
            var cards = new List<LearningModuleCardViewModel>();

            foreach (var module in tierModules)
            {
                var isLocked = IsModuleLocked(module.DifficultyLevel, currentLevel);
                var isCompleted = completedModuleIds.Contains(module.Id);
                var bestScore = moduleScores.GetValueOrDefault(module.Id, 0);
                var hasQuiz = module.QuizSets.Any(q => q.QuizItems.Any());

                var card = new LearningModuleCardViewModel
                {
                    Id = module.Id,
                    Title = module.Title,
                    Description = module.Description,
                    Category = module.Category,
                    DifficultyLevel = module.DifficultyLevel,
                    IsLocked = isLocked,
                    IsCompleted = isCompleted,
                    ProgressPercent = isCompleted ? 100 : bestScore,
                    HasQuiz = hasQuiz,
                    IsRecommended = false
                };
                cards.Add(card);

                if (!isLocked && !isCompleted && hasQuiz && recommendedCard == null)
                {
                    recommendedCard = card;
                }
            }

            tiers.Add(new DifficultyTierViewModel
            {
                Level = level,
                IsUnlocked = tierUnlocked,
                IsCurrentTier = level == currentLevel,
                Modules = cards
            });
        }

        if (recommendedCard != null)
            recommendedCard.IsRecommended = true;

        var progression = new PathProgressionViewModel
        {
            CurrentLevel = currentLevel,
            RecommendedLevel = currentLevel,
            EasyUnlocked = true,
            MediumUnlocked = !IsModuleLocked(DifficultyLevel.Medium, currentLevel),
            HardUnlocked = !IsModuleLocked(DifficultyLevel.Hard, currentLevel),
            RecommendedModuleId = recommendedCard?.Id,
            RecommendedModuleTitle = recommendedCard?.Title
        };

        return new PathBuildResult
        {
            CurrentLevel = currentLevel,
            CompletedModulesCount = completedCount,
            AverageScore = averageScore,
            Tiers = tiers,
            Progression = progression
        };
    }

    private async Task<HashSet<int>> GetCompletedModuleIdsAsync(int learnerId)
    {
        var completed = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet)
            .Where(a => a.LearnerId == learnerId && a.Score >= 80)
            .Select(a => a.QuizSet.LearningModuleId)
            .Distinct()
            .ToListAsync();

        return completed.ToHashSet();
    }

    private static bool IsModuleLocked(DifficultyLevel moduleLevel, DifficultyLevel currentLevel)
        => (int)moduleLevel > (int)currentLevel;

    private sealed class PathBuildResult
    {
        public DifficultyLevel CurrentLevel { get; set; }
        public int CompletedModulesCount { get; set; }
        public double AverageScore { get; set; }
        public List<DifficultyTierViewModel> Tiers { get; set; } = new();
        public PathProgressionViewModel Progression { get; set; } = new();
    }
}
