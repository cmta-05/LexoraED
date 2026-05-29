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
            if (role == "Admin")
                return RedirectToAction("Index", "AdminLearningModule");
            return RedirectToAction("Dashboard", "StudentLearning");
        }

        return RedirectToAction("Login", "LearnerAccess");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
