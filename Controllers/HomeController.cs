using LexoraED.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

public class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToAction("Login", "Account");

        if (User.IsInRole(LexoraRoles.Admin))
            return RedirectToAction("Index", "AdminAccount");
        if (User.IsInRole(LexoraRoles.Teacher))
            return RedirectToAction("Dashboard", "TeacherDashboard");
        return RedirectToAction("Dashboard", "StudentLearning");
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
