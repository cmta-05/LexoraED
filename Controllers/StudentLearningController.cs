using System.Security.Claims;
using LexoraED.Models;
using LexoraED.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

[Authorize(Roles = LexoraRoles.Student)]
public class StudentLearningController : Controller
{
    private readonly LearningPathPresentationService _learningPathPresentation;
    private readonly ActivityLogService _activityLogService;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentLearningController(
        LearningPathPresentationService learningPathPresentation,
        ActivityLogService activityLogService,
        UserManager<ApplicationUser> userManager)
    {
        _learningPathPresentation = learningPathPresentation;
        _activityLogService = activityLogService;
        _userManager = userManager;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private async Task<string> GetDisplayNameAsync()
    {
        var user = await _userManager.FindByIdAsync(UserId);
        return user?.FullName ?? User.Identity?.Name ?? "Learner";
    }

    public async Task<IActionResult> Dashboard()
    {
        return View(await _learningPathPresentation.BuildDashboardAsync(UserId, await GetDisplayNameAsync()));
    }

    public async Task<IActionResult> LearningPathMap()
    {
        return View(await _learningPathPresentation.BuildDashboardAsync(UserId, await GetDisplayNameAsync()));
    }

    public async Task<IActionResult> LearningModules(string? search, ModuleCategory? category, DifficultyLevel? difficulty)
    {
        return View(await _learningPathPresentation.BuildModulesPageAsync(UserId, search, category, difficulty));
    }

    public async Task<IActionResult> ProgressAnalytics()
    {
        return View(await _learningPathPresentation.BuildStudentAnalyticsAsync(UserId, await GetDisplayNameAsync()));
    }

    public async Task<IActionResult> StudyModule(int id)
    {
        var model = await _learningPathPresentation.BuildStudyModuleAsync(UserId, id);
        if (model == null) return NotFound();
        if (model.IsLocked)
        {
            TempData["LexoraMessage"] = "This module is locked. Complete prerequisite lessons with strong scores to unlock it.";
            return RedirectToAction(nameof(Dashboard));
        }
        await _activityLogService.LogAsync(UserId, "LessonOpened", $"Opened lesson: {model.Title}");
        return View(model);
    }

    public IActionResult BrowseModules() => RedirectToAction(nameof(LearningModules));
}
