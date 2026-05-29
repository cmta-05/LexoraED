using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class LearningPathPresentationService
{
    private readonly LexoraEDContext _context;
    private readonly GamificationService _gamificationService;

    public LearningPathPresentationService(LexoraEDContext context, GamificationService gamificationService)
    {
        _context = context;
        _gamificationService = gamificationService;
    }

    public async Task<LearningPathDashboardViewModel> BuildDashboardAsync(string userId, string learnerName)
    {
        var pathData = await BuildPathDataAsync(userId);
        var progress = await _context.LearningProgresses.AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);
        var badges = await _gamificationService.GetLearnerBadgesAsync(userId);
        var totalModules = pathData.Tiers.SelectMany(t => t.Modules).Count();
        var completed = pathData.Tiers.SelectMany(t => t.Modules).Count(m => m.IsCompleted);

        return new LearningPathDashboardViewModel
        {
            LearnerName = learnerName,
            CurrentLevel = pathData.CurrentLevel,
            CompletedModulesCount = pathData.CompletedModulesCount,
            AverageScore = pathData.AverageScore,
            ExperiencePoints = progress?.ExperiencePoints ?? 0,
            CurrentStreak = progress?.CurrentStreak ?? 0,
            OverallCompletionPercent = totalModules == 0 ? 0 : (int)Math.Round((double)completed / totalModules * 100),
            DifficultyTiers = pathData.Tiers,
            PathProgression = pathData.Progression,
            PathMap = pathData.PathMap,
            RecommendedModule = pathData.Tiers.SelectMany(t => t.Modules).FirstOrDefault(m => m.IsRecommended),
            Badges = badges.Select(b => new LearnerBadgeViewModel
            {
                Title = b.AchievementBadge.Title,
                Description = b.AchievementBadge.Description,
                EarnedAt = b.EarnedAt
            }).ToList()
        };
    }

    public async Task<LearningModulesPageViewModel> BuildModulesPageAsync(
        string userId, string? search, ModuleCategory? category, DifficultyLevel? difficulty)
    {
        var pathData = await BuildPathDataAsync(userId);
        var tiers = pathData.Tiers;

        if (!string.IsNullOrWhiteSpace(search) || category.HasValue || difficulty.HasValue)
        {
            foreach (var tier in tiers)
            {
                tier.Modules = tier.Modules.Where(m =>
                {
                    var matchSearch = string.IsNullOrWhiteSpace(search) ||
                        m.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        m.Description.Contains(search, StringComparison.OrdinalIgnoreCase);
                    var matchCat = !category.HasValue || m.Category == category.Value;
                    var matchDiff = !difficulty.HasValue || m.DifficultyLevel == difficulty.Value;
                    return matchSearch && matchCat && matchDiff;
                }).ToList();
            }
        }

        return new LearningModulesPageViewModel
        {
            DifficultyTiers = tiers,
            PathProgression = pathData.Progression,
            Search = search,
            CategoryFilter = category,
            DifficultyFilter = difficulty
        };
    }

    public async Task<StudyModuleViewModel?> BuildStudyModuleAsync(string userId, int moduleId)
    {
        var module = await _context.LearningModules
            .AsNoTracking()
            .Include(m => m.QuizSets).ThenInclude(q => q.QuizItems)
            .FirstOrDefaultAsync(m => m.Id == moduleId);

        if (module == null) return null;

        var pathData = await BuildPathDataAsync(userId);
        var card = pathData.Tiers.SelectMany(t => t.Modules).FirstOrDefault(m => m.Id == moduleId);

        return new StudyModuleViewModel
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Content = module.Content,
            Category = module.Category,
            DifficultyLevel = module.DifficultyLevel,
            HasQuiz = module.QuizSets.Any(q => q.QuizItems.Any()),
            IsLocked = card?.IsLocked ?? true
        };
    }

    public async Task<QuizCenterViewModel> BuildQuizCenterAsync(string userId)
    {
        var pathData = await BuildPathDataAsync(userId);
        var modules = pathData.Tiers.SelectMany(t => t.Modules).Where(m => m.HasQuiz).ToList();
        var quizSets = await _context.QuizSets.AsNoTracking().Include(q => q.QuizItems)
            .Where(q => modules.Select(m => m.Id).Contains(q.LearningModuleId)).ToListAsync();
        var attempts = await _context.LearningAttempts.AsNoTracking().Include(a => a.QuizSet)
            .Where(a => a.UserId == userId).ToListAsync();

        return new QuizCenterViewModel
        {
            AvailableQuizzes = modules.Select(m =>
            {
                var qs = quizSets.FirstOrDefault(q => q.LearningModuleId == m.Id);
                return new QuizCenterItemViewModel
                {
                    ModuleId = m.Id,
                    ModuleTitle = m.Title,
                    Category = m.Category,
                    DifficultyLevel = m.DifficultyLevel,
                    QuestionCount = qs?.QuizItems.Count ?? 0,
                    BestScore = attempts.Where(a => a.QuizSet.LearningModuleId == m.Id).Select(a => (int?)a.Score).DefaultIfEmpty().Max(),
                    IsLocked = m.IsLocked
                };
            }).ToList()
        };
    }

    public async Task<StudentProgressAnalyticsViewModel> BuildStudentAnalyticsAsync(string userId, string learnerName)
    {
        var pathData = await BuildPathDataAsync(userId);
        var attempts = await _context.LearningAttempts.AsNoTracking()
            .Include(a => a.QuizSet).ThenInclude(q => q.LearningModule)
            .Where(a => a.UserId == userId).OrderByDescending(a => a.AttemptDate).ToListAsync();
        var allModules = await _context.LearningModules.AsNoTracking().ToListAsync();
        var completedModuleIds = await GetCompletedModuleIdsAsync(userId);

        var categoryProgress = Enum.GetValues<ModuleCategory>().Select(category =>
        {
            var inCategory = allModules.Where(m => m.Category == category).ToList();
            var completed = inCategory.Count(m => completedModuleIds.Contains(m.Id));
            var categoryAttempts = attempts.Where(a => a.QuizSet.LearningModule.Category == category).ToList();
            return new CategoryProgressViewModel
            {
                Category = category,
                TotalModules = inCategory.Count,
                CompletedModules = completed,
                ProgressPercent = inCategory.Count == 0 ? 0 : (int)Math.Round((double)completed / inCategory.Count * 100),
                AverageScore = categoryAttempts.Count == 0 ? 0 : Math.Round(categoryAttempts.Average(a => a.Score), 1)
            };
        }).ToList();

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
        var studentRoleId = await _context.Roles.AsNoTracking()
            .Where(r => r.Name == LexoraRoles.Student)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        var studentIds = await _context.UserRoles.AsNoTracking()
            .Where(ur => ur.RoleId == studentRoleId)
            .Select(ur => ur.UserId)
            .ToListAsync();

        var students = await _context.Users.AsNoTracking()
            .Include(u => u.LearningProgress)
            .Where(u => studentIds.Contains(u.Id))
            .ToListAsync();

        var attempts = await _context.LearningAttempts.AsNoTracking()
            .Include(a => a.QuizSet).ThenInclude(q => q.LearningModule).ToListAsync();

        var learnerSummaries = students.Select(s =>
        {
            var learnerAttempts = attempts.Where(a => a.UserId == s.Id).ToList();
            return new AdminLearnerSummaryViewModel
            {
                UserId = s.Id,
                FullName = s.FullName,
                Username = s.UserName ?? "",
                CurrentLevel = s.LearningProgress?.CurrentLevel ?? DifficultyLevel.Beginner,
                CompletedModulesCount = s.LearningProgress?.CompletedModulesCount ?? 0,
                AverageScore = learnerAttempts.Count == 0 ? 0 : Math.Round(learnerAttempts.Average(a => a.Score), 1)
            };
        }).OrderByDescending(l => l.AverageScore).ToList();

        var categoryOverview = Enum.GetValues<ModuleCategory>().Select(category =>
        {
            var categoryAttempts = attempts.Where(a => a.QuizSet.LearningModule.Category == category).ToList();
            var avg = categoryAttempts.Count == 0 ? 0 : Math.Round(categoryAttempts.Average(a => a.Score), 1);
            return new CategoryWeaknessViewModel { Category = category, AverageScore = avg, AttemptCount = categoryAttempts.Count };
        }).ToList();

        var minAvg = categoryOverview.Where(c => c.AttemptCount > 0).Select(c => c.AverageScore).DefaultIfEmpty(100).Min();
        foreach (var item in categoryOverview.Where(c => c.AttemptCount > 0))
            item.IsWeakArea = item.AverageScore <= minAvg + 5;

        return new AdminProgressAnalyticsViewModel
        {
            OverallAverageScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(a => a.Score), 1),
            TotalLearners = students.Count,
            Learners = learnerSummaries,
            CategoryOverview = categoryOverview
        };
    }

    private async Task<PathBuildResult> BuildPathDataAsync(string userId)
    {
        var progress = await _context.LearningProgresses.AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);

        var currentLevel = progress?.CurrentLevel ?? DifficultyLevel.Beginner;
        var modules = await _context.LearningModules.AsNoTracking()
            .Include(m => m.QuizSets).ThenInclude(q => q.QuizItems)
            .OrderBy(m => m.DifficultyLevel).ThenBy(m => m.SortOrder).ThenBy(m => m.Title).ToListAsync();

        var attempts = await _context.LearningAttempts.AsNoTracking().Include(a => a.QuizSet)
            .Where(a => a.UserId == userId).ToListAsync();
        var completedModuleIds = await GetCompletedModuleIdsAsync(userId);
        var moduleProgressList = await _context.ModuleProgresses.AsNoTracking()
            .Where(p => p.UserId == userId).ToListAsync();

        var averageScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(a => a.Score), 1);
        var moduleScores = modules.ToDictionary(m => m.Id, m =>
            attempts.Where(a => a.QuizSet.LearningModuleId == m.Id).Select(a => a.Score).DefaultIfEmpty(0).Max());

        var tiers = new List<DifficultyTierViewModel>();
        LearningModuleCardViewModel? recommendedCard = null;
        var pathNodes = new List<PathMapNodeViewModel>();

        foreach (var level in new[] { DifficultyLevel.Beginner, DifficultyLevel.Intermediate, DifficultyLevel.Advanced })
        {
            var tierModules = modules.Where(m => m.DifficultyLevel == level).OrderBy(m => m.SortOrder).ToList();
            var cards = new List<LearningModuleCardViewModel>();

            foreach (var module in tierModules)
            {
                var isLocked = IsModuleLocked(module, currentLevel, completedModuleIds, moduleProgressList);
                var isCompleted = completedModuleIds.Contains(module.Id) ||
                    moduleProgressList.Any(p => p.LearningModuleId == module.Id && p.IsCompleted);
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
                    HasQuiz = hasQuiz
                };
                cards.Add(card);

                if (!isLocked && !isCompleted && hasQuiz && recommendedCard == null)
                    recommendedCard = card;

                var nodeState = isCompleted ? PathMapNodeState.Completed
                    : isLocked ? PathMapNodeState.Locked
                    : card.IsRecommended ? PathMapNodeState.Recommended
                    : PathMapNodeState.Current;

                pathNodes.Add(new PathMapNodeViewModel
                {
                    ModuleId = module.Id,
                    Title = module.Title,
                    Level = module.DifficultyLevel,
                    SortOrder = module.SortOrder,
                    State = recommendedCard?.Id == module.Id ? PathMapNodeState.Recommended : nodeState
                });
            }

            tiers.Add(new DifficultyTierViewModel
            {
                Level = level,
                IsUnlocked = !IsTierLocked(level, currentLevel),
                IsCurrentTier = level == currentLevel,
                Modules = cards
            });
        }

        if (recommendedCard != null)
        {
            recommendedCard.IsRecommended = true;
            var node = pathNodes.FirstOrDefault(n => n.ModuleId == recommendedCard.Id);
            if (node != null) node.State = PathMapNodeState.Recommended;
        }

        return new PathBuildResult
        {
            CurrentLevel = currentLevel,
            CompletedModulesCount = progress?.CompletedModulesCount ?? completedModuleIds.Count,
            AverageScore = averageScore,
            Tiers = tiers,
            Progression = new PathProgressionViewModel
            {
                CurrentLevel = currentLevel,
                RecommendedLevel = currentLevel,
                BeginnerUnlocked = true,
                IntermediateUnlocked = !IsTierLocked(DifficultyLevel.Intermediate, currentLevel),
                AdvancedUnlocked = !IsTierLocked(DifficultyLevel.Advanced, currentLevel),
                RecommendedModuleId = recommendedCard?.Id,
                RecommendedModuleTitle = recommendedCard?.Title
            },
            PathMap = new LearningPathMapViewModel { Nodes = pathNodes }
        };
    }

    private async Task<HashSet<int>> GetCompletedModuleIdsAsync(string userId)
    {
        var fromAttempts = await _context.LearningAttempts.AsNoTracking().Include(a => a.QuizSet)
            .Where(a => a.UserId == userId && a.Score >= 76)
            .Select(a => a.QuizSet.LearningModuleId).Distinct().ToListAsync();
        var fromProgress = await _context.ModuleProgresses.AsNoTracking()
            .Where(p => p.UserId == userId && p.IsCompleted)
            .Select(p => p.LearningModuleId).ToListAsync();
        return fromAttempts.Union(fromProgress).ToHashSet();
    }

    private static bool IsTierLocked(DifficultyLevel tier, DifficultyLevel current)
        => tier.TierOrder() > current.TierOrder();

    private static bool IsModuleLocked(
        LearningModule module,
        DifficultyLevel currentLevel,
        HashSet<int> completedModuleIds,
        List<ModuleProgress> moduleProgresses)
    {
        if (module.DifficultyLevel.TierOrder() > currentLevel.TierOrder())
            return true;

        if (module.PrerequisiteModuleId.HasValue &&
            !completedModuleIds.Contains(module.PrerequisiteModuleId.Value))
        {
            var prereqDone = moduleProgresses.Any(p =>
                p.LearningModuleId == module.PrerequisiteModuleId && p.IsCompleted);
            if (!prereqDone)
                return true;
        }

        if (module.SortOrder > 1)
        {
            var priorSort = module.SortOrder - 1;
            // Prerequisite chain within same tier handled by PrerequisiteModuleId in seed
        }

        return false;
    }

    private sealed class PathBuildResult
    {
        public DifficultyLevel CurrentLevel { get; set; }
        public int CompletedModulesCount { get; set; }
        public double AverageScore { get; set; }
        public List<DifficultyTierViewModel> Tiers { get; set; } = new();
        public PathProgressionViewModel Progression { get; set; } = new();
        public LearningPathMapViewModel PathMap { get; set; } = new();
    }
}
