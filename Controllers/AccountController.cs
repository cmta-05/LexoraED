using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly LexoraEDContext _context;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        LexoraEDContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToHome();
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByNameAsync(model.LoginInput.Trim())
            ?? await _userManager.FindByEmailAsync(model.LoginInput.Trim());

        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        if (user.AccountStatus == AccountStatus.Rejected)
        {
            ModelState.AddModelError(string.Empty, "Your account has been rejected. Contact the administrator.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        if (await _userManager.IsInRoleAsync(user, LexoraRoles.Teacher) &&
            user.AccountStatus == AccountStatus.Pending)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PendingApproval));
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToHome();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToHome();
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (model.RegisterAs != LexoraRoles.Student && model.RegisterAs != LexoraRoles.Teacher)
        {
            ModelState.AddModelError(nameof(model.RegisterAs), "Invalid registration type.");
            return View(model);
        }

        if (!ModelState.IsValid)
            return View(model);

        if (await _userManager.FindByNameAsync(model.Username.Trim()) != null)
        {
            ModelState.AddModelError(nameof(model.Username), "Username is already taken.");
            return View(model);
        }

        if (await _userManager.FindByEmailAsync(model.Email.Trim()) != null)
        {
            ModelState.AddModelError(nameof(model.Email), "Email is already registered.");
            return View(model);
        }

        if (model.RegisterAs == LexoraRoles.Student)
        {
            if (string.IsNullOrWhiteSpace(model.StudentIdNumber))
            {
                ModelState.AddModelError(nameof(model.StudentIdNumber), "Student ID is required.");
                return View(model);
            }
            if (await _context.StudentProfiles.AnyAsync(s => s.StudentIdNumber == model.StudentIdNumber.Trim()))
            {
                ModelState.AddModelError(nameof(model.StudentIdNumber), "Student ID is already registered.");
                return View(model);
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(model.TeacherIdNumber) ||
                string.IsNullOrWhiteSpace(model.SchoolName) ||
                string.IsNullOrWhiteSpace(model.Department) ||
                string.IsNullOrWhiteSpace(model.SubjectSpecialization) ||
                string.IsNullOrWhiteSpace(model.ContactNumber))
            {
                ModelState.AddModelError(string.Empty, "All teacher fields are required.");
                return View(model);
            }
            if (await _context.TeacherProfiles.AnyAsync(t => t.TeacherIdNumber == model.TeacherIdNumber!.Trim()))
            {
                ModelState.AddModelError(nameof(model.TeacherIdNumber), "Teacher ID is already registered.");
                return View(model);
            }
        }

        var user = new ApplicationUser
        {
            UserName = model.Username.Trim(),
            Email = model.Email.Trim(),
            EmailConfirmed = true,
            FullName = model.FullName.Trim(),
            AccountStatus = model.RegisterAs == LexoraRoles.Student
                ? AccountStatus.Approved
                : AccountStatus.Pending,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
        {
            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, model.RegisterAs);

        if (model.RegisterAs == LexoraRoles.Student)
        {
            _context.StudentProfiles.Add(new StudentProfile
            {
                UserId = user.Id,
                StudentIdNumber = model.StudentIdNumber!.Trim(),
                LearningPreference = model.LearningPreference
            });
            _context.LearningProgresses.Add(new LearningProgress
            {
                UserId = user.Id,
                CurrentLevel = DifficultyLevel.Beginner,
                CompletedModulesCount = 0
            });
            await _context.SaveChangesAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Dashboard", "StudentLearning");
        }

        _context.TeacherProfiles.Add(new TeacherProfile
        {
            UserId = user.Id,
            TeacherIdNumber = model.TeacherIdNumber!.Trim(),
            SchoolName = model.SchoolName!.Trim(),
            Department = model.Department!.Trim(),
            SubjectSpecialization = model.SubjectSpecialization!.Trim(),
            ContactNumber = model.ContactNumber!.Trim()
        });
        await _context.SaveChangesAsync();

        TempData["LexoraMessage"] = "Waiting for admin approval.";
        return RedirectToAction(nameof(PendingApproval));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult PendingApproval() => View();

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    private IActionResult RedirectToHome()
    {
        if (User.IsInRole(LexoraRoles.Admin))
            return RedirectToAction("Index", "AdminAccount");
        if (User.IsInRole(LexoraRoles.Teacher))
            return RedirectToAction("Dashboard", "TeacherDashboard");
        return RedirectToAction("Dashboard", "StudentLearning");
    }
}
