using LexoraED.Data;
using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Controllers;

[RequireLearnerRole(LearnerRole.Admin)]
public class AdminLearningModuleController : Controller
{
    private readonly LexoraEDContext _context;
    private readonly LearningPathPresentationService _learningPathPresentation;

    public AdminLearningModuleController(
        LexoraEDContext context,
        LearningPathPresentationService learningPathPresentation)
    {
        _context = context;
        _learningPathPresentation = learningPathPresentation;
    }

    public async Task<IActionResult> ProgressAnalytics()
    {
        var model = await _learningPathPresentation.BuildAdminAnalyticsAsync();
        return View(model);
    }

    public async Task<IActionResult> Index()
    {
        var modules = await _context.LearningModules
            .AsNoTracking()
            .Include(m => m.QuizSets)
            .OrderBy(m => m.DifficultyLevel)
            .ThenBy(m => m.Title)
            .ToListAsync();

        return View(modules);
    }

    public IActionResult Create()
    {
        return View(new LearningModuleFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LearningModuleFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var module = new LearningModule
        {
            Title = model.Title,
            Description = model.Description,
            Content = model.Content,
            Category = model.Category,
            DifficultyLevel = model.DifficultyLevel
        };

        _context.LearningModules.Add(module);
        await _context.SaveChangesAsync();

        _context.QuizSets.Add(new QuizSet { LearningModuleId = module.Id });
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var module = await _context.LearningModules.FindAsync(id);
        if (module == null)
            return NotFound();

        return View(new LearningModuleFormViewModel
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Content = module.Content,
            Category = module.Category,
            DifficultyLevel = module.DifficultyLevel
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LearningModuleFormViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var module = await _context.LearningModules.FindAsync(id);
        if (module == null)
            return NotFound();

        module.Title = model.Title;
        module.Description = model.Description;
        module.Content = model.Content;
        module.Category = model.Category;
        module.DifficultyLevel = model.DifficultyLevel;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var module = await _context.LearningModules.FindAsync(id);
        if (module != null)
        {
            _context.LearningModules.Remove(module);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
