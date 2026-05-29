using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

[RequireLearnerRole(LearnerRole.Admin)]
public class AdminLearnerManagementController : Controller
{
    private readonly LearnerManagementService _learnerManagement;

    public AdminLearnerManagementController(LearnerManagementService learnerManagement)
    {
        _learnerManagement = learnerManagement;
    }

    public async Task<IActionResult> Index(string? search, LearnerRole? role, int page = 1)
    {
        return View(await _learnerManagement.GetLearnersAsync(search, role, page));
    }

    public IActionResult Create() => View(new LearnerFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LearnerFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var (success, error) = await _learnerManagement.CreateLearnerAsync(model);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, error ?? "Create failed.");
            return View(model);
        }
        TempData["LexoraMessage"] = "User created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var learner = await _learnerManagement.GetLearnerAsync(id);
        if (learner == null) return NotFound();
        return View(new LearnerFormViewModel
        {
            Id = learner.Id,
            FullName = learner.FullName,
            Username = learner.Username,
            Role = learner.Role,
            IsActive = learner.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LearnerFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var (success, error) = await _learnerManagement.UpdateLearnerAsync(model);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, error ?? "Update failed.");
            return View(model);
        }
        TempData["LexoraMessage"] = "User updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var learner = await _learnerManagement.GetLearnerAsync(id);
        if (learner == null) return NotFound();
        return View(new LearnerDetailsViewModel
        {
            Learner = new LearnerCardViewModel
            {
                Id = learner.Id,
                FullName = learner.FullName,
                Username = learner.Username,
                Role = learner.Role,
                IsActive = learner.IsActive,
                CreatedAt = learner.CreatedAt
            },
            Progress = learner.LearningProgress
        });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var learner = await _learnerManagement.GetLearnerAsync(id);
        if (learner == null) return NotFound();
        return View(new LearnerCardViewModel
        {
            Id = learner.Id,
            FullName = learner.FullName,
            Username = learner.Username,
            Role = learner.Role,
            IsActive = learner.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _learnerManagement.DeleteLearnerAsync(id);
        TempData["LexoraMessage"] = "User deleted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(int id, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            TempData["LexoraMessage"] = "Password cannot be empty.";
            return RedirectToAction(nameof(Edit), new { id });
        }
        await _learnerManagement.ResetPasswordAsync(id, newPassword);
        TempData["LexoraMessage"] = "Password reset successfully.";
        return RedirectToAction(nameof(Edit), new { id });
    }
}
