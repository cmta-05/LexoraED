using LexoraED.Data;
using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Controllers;

[RequireLearnerRole(LearnerRole.Student)]
public class StudentLearningController : Controller
{
    private readonly LexoraEDContext _context;
    private readonly LearningPathPresentationService _learningPathPresentation;

    public StudentLearningController(
        LexoraEDContext context,
        LearningPathPresentationService learningPathPresentation)
    {
        _context = context;
        _learningPathPresentation = learningPathPresentation;
    }

    public async Task<IActionResult> Dashboard()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var learnerName = HttpContext.Session.GetString(LearnerSessionKeys.LearnerFullName) ?? "Learner";
        var model = await _learningPathPresentation.BuildDashboardAsync(learnerId, learnerName);
        return View(model);
    }

    public async Task<IActionResult> LearningModules()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var model = await _learningPathPresentation.BuildModulesPageAsync(learnerId);
        return View(model);
    }

    public async Task<IActionResult> ProgressAnalytics()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var learnerName = HttpContext.Session.GetString(LearnerSessionKeys.LearnerFullName) ?? "Learner";
        var model = await _learningPathPresentation.BuildStudentAnalyticsAsync(learnerId, learnerName);
        return View(model);
    }

    public async Task<IActionResult> StudyModule(int id)
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var model = await _learningPathPresentation.BuildStudyModuleAsync(learnerId, id);

        if (model == null)
            return NotFound();

        if (model.IsLocked)
        {
            TempData["LexoraMessage"] = "This module is locked. Complete easier modules and improve your scores to unlock it.";
            return RedirectToAction(nameof(Dashboard));
        }

        return View(model);
    }

    public IActionResult BrowseModules() => RedirectToAction(nameof(LearningModules));
}
