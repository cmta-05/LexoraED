using LexoraED.Models;
using LexoraED.Services;
using Microsoft.AspNetCore.Mvc;

namespace LexoraED.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId).HasValue)
        {
            var role = HttpContext.Session.GetString(LearnerSessionKeys.LearnerRole);
            return role switch
            {
                nameof(LearnerRole.Admin) => RedirectToAction("Index", "AdminLearnerManagement"),
                nameof(LearnerRole.Teacher) => RedirectToAction("Dashboard", "TeacherDashboard"),
                _ => RedirectToAction("Dashboard", "StudentLearning")
            };
        }

        return RedirectToAction("Login", "LearnerAccess");
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
