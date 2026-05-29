using System.Security.Claims;
using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

public class LexoraReportsController : Controller
{
    private readonly LexoraReportService _reports;

    public LexoraReportsController(LexoraReportService reports)
    {
        _reports = reports;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [Authorize(Roles = LexoraRoles.Student)]
    public async Task<IActionResult> StudentReport(DateTime? from, DateTime? to)
    {
        return View(await _reports.BuildStudentReportAsync(UserId, from, to));
    }

    [Authorize(Roles = LexoraRoles.Student)]
    public async Task<IActionResult> ExportStudentReport(DateTime? from, DateTime? to)
    {
        var report = await _reports.BuildStudentReportAsync(UserId, from, to);
        var body = $"<h1>Student Report — {report.LearnerName}</h1><p>Average: {report.Analytics.AverageScore}%</p><p>Level: {report.Analytics.CurrentLevel}</p>";
        var bytes = _reports.GeneratePrintableHtmlPdfBytes("Student Report", body);
        return File(bytes, "text/html", $"LexoraED-Student-Report-{DateTime.Now:yyyyMMdd}.html");
    }

    [Authorize(Roles = LexoraRoles.Teacher)]
    [RequireApprovedTeacher]
    public async Task<IActionResult> TeacherReport(DateTime? from, DateTime? to)
    {
        return View(await _reports.BuildTeacherReportAsync(from, to));
    }

    [Authorize(Roles = LexoraRoles.Teacher)]
    [RequireApprovedTeacher]
    public async Task<IActionResult> ExportTeacherReport(DateTime? from, DateTime? to)
    {
        var report = await _reports.BuildTeacherReportAsync(from, to);
        var body = $"<h1>Teacher Class Report</h1><p>Students tracked: {report.Analytics.StudentSummaries.Count}</p>";
        var bytes = _reports.GeneratePrintableHtmlPdfBytes("Teacher Report", body);
        return File(bytes, "text/html", $"LexoraED-Teacher-Report-{DateTime.Now:yyyyMMdd}.html");
    }

    [Authorize(Roles = LexoraRoles.Admin)]
    public async Task<IActionResult> AdminReport()
    {
        return View(await _reports.BuildAdminReportAsync());
    }

    [Authorize(Roles = LexoraRoles.Admin)]
    public async Task<IActionResult> ExportAdminReport()
    {
        var report = await _reports.BuildAdminReportAsync();
        var body = $"<h1>Platform Report</h1><p>Users: {report.TotalUsers}</p><p>Average Score: {report.PlatformAverageScore}%</p>";
        var bytes = _reports.GeneratePrintableHtmlPdfBytes("Admin Report", body);
        return File(bytes, "text/html", $"LexoraED-Admin-Report-{DateTime.Now:yyyyMMdd}.html");
    }
}
