using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

[RequireLearnerRole(LearnerRole.Teacher)]
public class TeacherDashboardController : Controller
{
    private readonly TeacherInsightsService _teacherInsights;

    public TeacherDashboardController(TeacherInsightsService teacherInsights)
    {
        _teacherInsights = teacherInsights;
    }

    public async Task<IActionResult> Dashboard()
    {
        return View(await _teacherInsights.BuildDashboardAsync());
    }

    public async Task<IActionResult> ProgressAnalytics()
    {
        return View(await _teacherInsights.BuildAnalyticsAsync());
    }
}
