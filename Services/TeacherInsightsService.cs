using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class TeacherInsightsService
{
    private readonly LexoraEDContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeacherInsightsService(LexoraEDContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<TeacherDashboardViewModel> BuildDashboardAsync()
    {
        var students = await GetStudentsAsync();

        var attempts = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet).ThenInclude(q => q.LearningModule)
            .Include(a => a.User)
            .OrderByDescending(a => a.AttemptDate)
            .Take(50)
            .ToListAsync();

        var avgScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(a => a.Score), 1);

        var categoryScores = attempts
            .GroupBy(a => a.QuizSet.LearningModule.Category)
            .Select(g => new { Category = g.Key, Avg = g.Average(a => a.Score) })
            .OrderBy(x => x.Avg)
            .ToList();

        var weakest = categoryScores.FirstOrDefault();
        var totalModules = await _context.LearningModules.CountAsync();
        var completedProgress = await _context.ModuleProgresses.CountAsync(p => p.IsCompleted);
        var completionRate = students.Count == 0 || totalModules == 0
            ? 0
            : Math.Round((double)completedProgress / (students.Count * totalModules) * 100, 1);

        return new TeacherDashboardViewModel
        {
            TotalStudents = students.Count,
            AverageStudentScore = avgScore,
            MostDifficultCategory = weakest?.Category.ToString() ?? "N/A",
            LearningCompletionRate = completionRate,
            RecentActivity = attempts.Take(8).Select(a => new TeacherActivityItemViewModel
            {
                StudentName = a.User.FullName,
                ModuleTitle = a.QuizSet.LearningModule.Title,
                Score = a.Score,
                AttemptDate = a.AttemptDate
            }).ToList(),
            CategoryChartData = categoryScores.Select(c => new ChartDataPointViewModel
            {
                Label = c.Category.ToString(),
                Value = Math.Round(c.Avg, 1)
            }).ToList(),
            ScoreTrendData = attempts
                .GroupBy(a => a.AttemptDate.Date)
                .OrderBy(g => g.Key)
                .TakeLast(7)
                .Select(g => new ChartDataPointViewModel
                {
                    Label = g.Key.ToString("MMM d"),
                    Value = Math.Round(g.Average(a => a.Score), 1)
                }).ToList()
        };
    }

    public async Task<TeacherAnalyticsViewModel> BuildAnalyticsAsync()
    {
        var dashboard = await BuildDashboardAsync();
        var students = await GetStudentsAsync();
        var attempts = await _context.LearningAttempts
            .AsNoTracking()
            .Include(a => a.QuizSet).ThenInclude(q => q.LearningModule)
            .ToListAsync();

        var summaries = students.Select(s =>
        {
            var studentAttempts = attempts.Where(a => a.UserId == s.Id).ToList();
            return new TeacherStudentSummaryViewModel
            {
                StudentId = s.Id,
                FullName = s.FullName,
                CurrentLevel = s.LearningProgress?.CurrentLevel ?? DifficultyLevel.Beginner,
                AverageScore = studentAttempts.Count == 0 ? 0 : Math.Round(studentAttempts.Average(a => a.Score), 1),
                CompletedModules = s.LearningProgress?.CompletedModulesCount ?? 0,
                RecommendedLevel = s.LearningProgress?.CurrentLevel ?? DifficultyLevel.Beginner
            };
        }).OrderByDescending(s => s.AverageScore).ToList();

        var weakCategories = Enum.GetValues<ModuleCategory>().Select(cat =>
        {
            var catAttempts = attempts.Where(a => a.QuizSet.LearningModule.Category == cat).ToList();
            var avg = catAttempts.Count == 0 ? 0 : catAttempts.Average(a => a.Score);
            return new CategoryWeaknessViewModel
            {
                Category = cat,
                AverageScore = Math.Round(avg, 1),
                AttemptCount = catAttempts.Count,
                IsWeakArea = false
            };
        }).ToList();

        var minAvg = weakCategories.Where(c => c.AttemptCount > 0).Select(c => c.AverageScore).DefaultIfEmpty(100).Min();
        foreach (var c in weakCategories.Where(c => c.AttemptCount > 0))
            c.IsWeakArea = c.AverageScore <= minAvg + 5;

        return new TeacherAnalyticsViewModel
        {
            Dashboard = dashboard,
            StudentSummaries = summaries,
            WeakCategories = weakCategories
        };
    }

    private async Task<List<ApplicationUser>> GetStudentsAsync()
    {
        var users = await _userManager.GetUsersInRoleAsync(LexoraRoles.Student);
        var ids = users.Select(u => u.Id).ToList();
        return await _context.Users
            .Where(u => ids.Contains(u.Id) && u.IsActive && u.AccountStatus == AccountStatus.Approved)
            .Include(u => u.LearningProgress)
            .ToListAsync();
    }
}
