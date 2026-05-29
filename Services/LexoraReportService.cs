using System.Text;
using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class LexoraReportService
{
    private readonly LexoraEDContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TeacherInsightsService _teacherInsights;
    private readonly LearningPathPresentationService _learningPath;

    public LexoraReportService(
        LexoraEDContext context,
        UserManager<ApplicationUser> userManager,
        TeacherInsightsService teacherInsights,
        LearningPathPresentationService learningPath)
    {
        _context = context;
        _userManager = userManager;
        _teacherInsights = teacherInsights;
        _learningPath = learningPath;
    }

    public async Task<StudentReportViewModel> BuildStudentReportAsync(string userId, DateTime? from, DateTime? to)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException("Student not found.");
        var analytics = await _learningPath.BuildStudentAnalyticsAsync(userId, user.FullName);
        var badges = await _context.LearnerAchievements.AsNoTracking()
            .Include(a => a.AchievementBadge)
            .Where(a => a.UserId == userId)
            .ToListAsync();

        var attemptsQuery = _context.LearningAttempts.AsNoTracking()
            .Include(a => a.QuizSet).ThenInclude(q => q.LearningModule)
            .Where(a => a.UserId == userId);

        if (from.HasValue)
            attemptsQuery = attemptsQuery.Where(a => a.AttemptDate >= from.Value);
        if (to.HasValue)
            attemptsQuery = attemptsQuery.Where(a => a.AttemptDate <= to.Value.AddDays(1));

        var attempts = await attemptsQuery.OrderByDescending(a => a.AttemptDate).ToListAsync();

        return new StudentReportViewModel
        {
            LearnerName = user.FullName,
            GeneratedAt = DateTime.Now,
            FromDate = from,
            ToDate = to,
            Analytics = analytics,
            QuizAttempts = attempts.Select(a => new RecentAttemptViewModel
            {
                ModuleTitle = a.QuizSet.LearningModule.Title,
                Score = a.Score,
                AttemptDate = a.AttemptDate
            }).ToList(),
            Badges = badges.Select(b => b.AchievementBadge.Title).ToList()
        };
    }

    public async Task<TeacherReportViewModel> BuildTeacherReportAsync(DateTime? from, DateTime? to)
    {
        var analytics = await _teacherInsights.BuildAnalyticsAsync();
        return new TeacherReportViewModel
        {
            GeneratedAt = DateTime.Now,
            FromDate = from,
            ToDate = to,
            Analytics = analytics
        };
    }

    public async Task<AdminReportViewModel> BuildAdminReportAsync()
    {
        var attempts = await _context.LearningAttempts.AsNoTracking().ToListAsync();
        var modules = await _context.LearningModules.AsNoTracking().CountAsync();
        var users = await _context.Users.AsNoTracking().ToListAsync();

        var studentCount = 0;
        var teacherCount = 0;
        var adminCount = 0;
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(LexoraRoles.Student)) studentCount++;
            if (roles.Contains(LexoraRoles.Teacher)) teacherCount++;
            if (roles.Contains(LexoraRoles.Admin)) adminCount++;
        }

        return new AdminReportViewModel
        {
            GeneratedAt = DateTime.Now,
            TotalUsers = users.Count,
            TotalStudents = studentCount,
            TotalTeachers = teacherCount,
            TotalAdmins = adminCount,
            TotalModules = modules,
            TotalQuizAttempts = attempts.Count,
            PlatformAverageScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(a => a.Score), 1),
            ActiveUsers = users.Count(u => u.IsActive)
        };
    }

    public byte[] GeneratePrintableHtmlPdfBytes(string htmlTitle, string htmlBody)
    {
        var html = $@"<!DOCTYPE html><html><head><meta charset='utf-8'><title>{htmlTitle}</title>
<style>body{{font-family:Segoe UI,Arial,sans-serif;padding:40px;color:#2A2A2A}}
h1{{color:#502D55}}table{{width:100%;border-collapse:collapse;margin-top:20px}}
td,th{{border:1px solid #ddd;padding:8px}}th{{background:#F6DBC0}}</style></head>
<body>{htmlBody}<p style='margin-top:40px;font-size:12px;color:#666'>LexoraED Report — {DateTime.Now:g}</p></body></html>";
        return Encoding.UTF8.GetBytes(html);
    }
}
