using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

public class LearnerAccessController : Controller
{
    private readonly LearnerAuthenticationService _authenticationService;

    public LearnerAccessController(LearnerAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId).HasValue)
            return RedirectToRoleHome();

        return View(new LearnerLoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LearnerLoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, errorMessage, learner) = await _authenticationService.AuthenticateLearnerAsync(
            model.Username, model.Password);

        if (!success || learner == null)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Login failed.");
            return View(model);
        }

        EstablishLearnerSession(learner);
        return RedirectToRoleHome(learner.Role);
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId).HasValue)
            return RedirectToRoleHome();

        return View(new LearnerRegistrationViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(LearnerRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, errorMessage, learner) = await _authenticationService.RegisterLearnerAsync(
            model.FullName,
            model.Username,
            model.Password,
            model.Role);

        if (!success || learner == null)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Registration failed.");
            return View(model);
        }

        EstablishLearnerSession(learner);
        return RedirectToRoleHome(learner.Role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    private void EstablishLearnerSession(Learner learner)
    {
        HttpContext.Session.SetInt32(LearnerSessionKeys.LearnerId, learner.Id);
        HttpContext.Session.SetString(LearnerSessionKeys.LearnerRole, learner.Role.ToString());
        HttpContext.Session.SetString(LearnerSessionKeys.LearnerFullName, learner.FullName);
    }

    private IActionResult RedirectToRoleHome(LearnerRole? role = null)
    {
        var roleString = role?.ToString() ?? HttpContext.Session.GetString(LearnerSessionKeys.LearnerRole);
        return roleString switch
        {
            nameof(LearnerRole.Admin) => RedirectToAction("Index", "AdminLearnerManagement"),
            nameof(LearnerRole.Teacher) => RedirectToAction("Dashboard", "TeacherDashboard"),
            _ => RedirectToAction("Dashboard", "StudentLearning")
        };
    }
}
