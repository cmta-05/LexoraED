using LexoraED.Data;
using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Controllers;

[Authorize(Roles = $"{LexoraRoles.Teacher},{LexoraRoles.Admin}")]
[RequireApprovedTeacher]
public class TeacherLearningModuleController : Controller
{
    private readonly LexoraEDContext _context;

    public TeacherLearningModuleController(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search, ModuleCategory? category, DifficultyLevel? difficulty)
    {
        var query = _context.LearningModules.AsNoTracking().Include(m => m.QuizSets).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.Title.Contains(search) || m.Description.Contains(search));
        if (category.HasValue)
            query = query.Where(m => m.Category == category.Value);
        if (difficulty.HasValue)
            query = query.Where(m => m.DifficultyLevel == difficulty.Value);

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Difficulty = difficulty;
        return View(await query.OrderBy(m => m.DifficultyLevel).ThenBy(m => m.SortOrder).ToListAsync());
    }

    public IActionResult Create() => View(new LearningModuleFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LearningModuleFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            var module = new LearningModule
            {
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                Category = model.Category,
                DifficultyLevel = model.DifficultyLevel,
                SortOrder = await _context.LearningModules.CountAsync(m => m.DifficultyLevel == model.DifficultyLevel) + 1
            };
            _context.LearningModules.Add(module);
            await _context.SaveChangesAsync();
            _context.QuizSets.Add(new QuizSet { LearningModuleId = module.Id });
            await _context.SaveChangesAsync();
            TempData["LexoraMessage"] = "Module created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unable to create module. Please try again.");
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var module = await _context.LearningModules.FindAsync(id);
        if (module == null) return NotFound();
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
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        var module = await _context.LearningModules.FindAsync(id);
        if (module == null) return NotFound();
        module.Title = model.Title;
        module.Description = model.Description;
        module.Content = model.Content;
        module.Category = model.Category;
        module.DifficultyLevel = model.DifficultyLevel;
        await _context.SaveChangesAsync();
        TempData["LexoraMessage"] = "Module updated successfully.";
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
