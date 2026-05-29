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
public class TeacherQuizManagementController : Controller
{
    private readonly LexoraEDContext _context;

    public TeacherQuizManagementController(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? moduleId)
    {
        var modules = await _context.LearningModules.AsNoTracking()
            .Include(m => m.QuizSets).ThenInclude(q => q.QuizItems)
            .OrderBy(m => m.DifficultyLevel).ThenBy(m => m.SortOrder)
            .ToListAsync();
        ViewBag.SelectedModuleId = moduleId;
        return View(modules);
    }

    public async Task<IActionResult> ManageQuestions(int moduleId)
    {
        var quizSet = await _context.QuizSets.Include(q => q.QuizItems).Include(q => q.LearningModule)
            .FirstOrDefaultAsync(q => q.LearningModuleId == moduleId);
        if (quizSet == null)
        {
            quizSet = new QuizSet { LearningModuleId = moduleId };
            _context.QuizSets.Add(quizSet);
            await _context.SaveChangesAsync();
            quizSet = await _context.QuizSets.Include(q => q.QuizItems).Include(q => q.LearningModule)
                .FirstAsync(q => q.Id == quizSet.Id);
        }
        ViewBag.ModuleId = moduleId;
        return View(quizSet);
    }

    public IActionResult AddQuestion(int quizSetId, int moduleId)
    {
        return View(new QuizItemFormViewModel { QuizSetId = quizSetId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestion(QuizItemFormViewModel model, int moduleId)
    {
        if (!ModelState.IsValid) return View(model);
        _context.QuizItems.Add(new QuizItem
        {
            QuizSetId = model.QuizSetId,
            QuestionType = model.QuestionType,
            QuestionText = model.QuestionText,
            ChoiceA = model.ChoiceA,
            ChoiceB = model.ChoiceB,
            ChoiceC = model.ChoiceC,
            ChoiceD = model.ChoiceD,
            CorrectAnswer = model.CorrectAnswer,
            Explanation = model.Explanation
        });
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageQuestions), new { moduleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuestion(int id, int moduleId)
    {
        var item = await _context.QuizItems.FindAsync(id);
        if (item != null)
        {
            _context.QuizItems.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageQuestions), new { moduleId });
    }
}
