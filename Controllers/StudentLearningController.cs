using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

[RequireLearnerRole(LearnerRole.Student)]
public class StudentLearningController : Controller
{
    private readonly LearningPathPresentationService _learningPathPresentation;
    private readonly ActivityLogService _activityLogService;

    public StudentLearningController(
        LearningPathPresentationService learningPathPresentation,
        ActivityLogService activityLogService)
    {
        _learningPathPresentation = learningPathPresentation;
        _activityLogService = activityLogService;
    }

    public async Task<IActionResult> Dashboard()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var learnerName = HttpContext.Session.GetString(LearnerSessionKeys.LearnerFullName) ?? "Learner";
        return View(await _learningPathPresentation.BuildDashboardAsync(learnerId, learnerName));
    }

    public async Task<IActionResult> LearningPathMap()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var learnerName = HttpContext.Session.GetString(LearnerSessionKeys.LearnerFullName) ?? "Learner";
        return View(await _learningPathPresentation.BuildDashboardAsync(learnerId, learnerName));
    }

    public async Task<IActionResult> LearningModules(string? search, ModuleCategory? category, DifficultyLevel? difficulty)
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        return View(await _learningPathPresentation.BuildModulesPageAsync(learnerId, search, category, difficulty));
    }

    public async Task<IActionResult> ProgressAnalytics()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var learnerName = HttpContext.Session.GetString(LearnerSessionKeys.LearnerFullName) ?? "Learner";
        return View(await _learningPathPresentation.BuildStudentAnalyticsAsync(learnerId, learnerName));
    }

    public async Task<IActionResult> StudyModule(int id)
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var model = await _learningPathPresentation.BuildStudyModuleAsync(learnerId, id);

        if (model == null)
            return NotFound();

        if (model.IsLocked)
        {
            TempData["LexoraMessage"] = "This module is locked. Complete prerequisite lessons with strong scores to unlock it.";
            return RedirectToAction(nameof(Dashboard));
        }

        await _activityLogService.LogAsync(learnerId, "LessonOpened", $"Opened lesson: {model.Title}");
        return View(model);
    }

    public IActionResult BrowseModules() => RedirectToAction(nameof(LearningModules));
}
