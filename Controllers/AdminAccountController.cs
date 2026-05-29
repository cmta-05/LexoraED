using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

[Authorize(Roles = LexoraRoles.Admin)]
public class AdminAccountController : Controller
{
    private readonly IdentityAdminService _adminService;

    public AdminAccountController(IdentityAdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index(string? search, string? role, AccountStatus? status, int page = 1)
    {
        return View(await _adminService.GetUsersAsync(search, role, status, page));
    }

    public async Task<IActionResult> Details(string id)
    {
        var model = await _adminService.GetUserDetailsAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveTeacher(string id)
    {
        await _adminService.ApproveTeacherAsync(id);
        TempData["LexoraMessage"] = "Teacher account approved.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectTeacher(string id)
    {
        await _adminService.RejectTeacherAsync(id);
        TempData["LexoraMessage"] = "Teacher account rejected.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(string id, bool isActive)
    {
        await _adminService.SetActiveAsync(id, isActive);
        TempData["LexoraMessage"] = isActive ? "Account activated." : "Account deactivated.";
        return RedirectToAction(nameof(Details), new { id });
    }

    public IActionResult CreateAdmin() => View(new AdminCreateViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdmin(AdminCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var (success, error) = await _adminService.CreateAdminAsync(model);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, error ?? "Could not create admin.");
            return View(model);
        }
        TempData["LexoraMessage"] = "Admin account created.";
        return RedirectToAction(nameof(Index));
    }
}
