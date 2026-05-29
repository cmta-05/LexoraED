using LexoraED.Data;
using LexoraED.Filters;
using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Controllers;

[RequireLearnerRole(LearnerRole.Student)]
public class QuizAssessmentController : Controller
{
    private readonly LexoraEDContext _context;
    private readonly AdaptiveLearningPathService _adaptiveLearningPathService;
    private readonly LearningPathPresentationService _learningPathPresentation;

    public QuizAssessmentController(
        LexoraEDContext context,
        AdaptiveLearningPathService adaptiveLearningPathService,
        LearningPathPresentationService learningPathPresentation)
    {
        _context = context;
        _adaptiveLearningPathService = adaptiveLearningPathService;
        _learningPathPresentation = learningPathPresentation;
    }

    public async Task<IActionResult> QuizCenter()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var model = await _learningPathPresentation.BuildQuizCenterAsync(learnerId);
        return View(model);
    }

    public async Task<IActionResult> TakeQuiz(int moduleId)
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        var studyModule = await _learningPathPresentation.BuildStudyModuleAsync(learnerId, moduleId);

        if (studyModule == null)
            return NotFound();

        if (studyModule.IsLocked)
        {
            TempData["LexoraMessage"] = "This quiz is locked until you advance your learning path.";
            return RedirectToAction("QuizCenter");
        }

        var quizSet = await _context.QuizSets
            .AsNoTracking()
            .Include(q => q.QuizItems)
            .Include(q => q.LearningModule)
            .FirstOrDefaultAsync(q => q.LearningModuleId == moduleId);

        if (quizSet == null || !quizSet.QuizItems.Any())
        {
            TempData["LexoraMessage"] = "No quiz is available for this module yet.";
            return RedirectToAction("StudyModule", "StudentLearning", new { id = moduleId });
        }

        var model = new QuizSubmissionViewModel
        {
            QuizSetId = quizSet.Id,
            ModuleId = moduleId,
            ModuleTitle = quizSet.LearningModule.Title,
            Answers = quizSet.QuizItems.OrderBy(i => i.Id).Select(item => new QuizAnswerViewModel
            {
                QuizItemId = item.Id,
                QuestionText = item.QuestionText,
                ChoiceA = item.ChoiceA,
                ChoiceB = item.ChoiceB,
                ChoiceC = item.ChoiceC,
                ChoiceD = item.ChoiceD
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitQuiz(QuizSubmissionViewModel model)
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;

        var quizItems = await _context.QuizItems
            .AsNoTracking()
            .Where(q => q.QuizSetId == model.QuizSetId)
            .ToListAsync();

        if (!quizItems.Any())
            return NotFound();

        if (model.Answers == null || model.Answers.Count != quizItems.Count)
        {
            ModelState.AddModelError(string.Empty, "Invalid quiz submission. Please try again.");
            return await RetakeQuizView(model);
        }

        var unanswered = model.Answers
            .Where(a => string.IsNullOrWhiteSpace(a.SelectedAnswer))
            .ToList();

        if (unanswered.Any())
        {
            ModelState.AddModelError(string.Empty,
                $"Please answer all questions. {unanswered.Count} question(s) still need a response.");
            return await RetakeQuizView(model);
        }

        var correctCount = 0;
        foreach (var answer in model.Answers)
        {
            var item = quizItems.FirstOrDefault(q => q.Id == answer.QuizItemId);
            if (item != null &&
                string.Equals(answer.SelectedAnswer, item.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
            {
                correctCount++;
            }
        }

        var score = (int)Math.Round((double)correctCount / quizItems.Count * 100);

        var evaluation = await _adaptiveLearningPathService.EvaluateLearnerPerformanceAsync(
            learnerId, model.QuizSetId, score);

        var (tier, headline) = QuizFeedbackHelper.GetFeedback(score);

        var module = await _context.QuizSets
            .AsNoTracking()
            .Where(q => q.Id == model.QuizSetId)
            .Select(q => new { q.LearningModuleId, q.LearningModule.Title })
            .FirstOrDefaultAsync();

        TempData["QuizSuccess"] = true;

        var result = new QuizResultViewModel
        {
            Score = evaluation.Score,
            GuidanceMessage = evaluation.GuidanceMessage,
            FeedbackTier = tier,
            FeedbackHeadline = headline,
            RecommendedLevel = evaluation.RecommendedLevel,
            CurrentLevel = evaluation.CurrentLevel,
            CompletedModulesCount = evaluation.CompletedModulesCount,
            ModuleId = module?.LearningModuleId ?? model.ModuleId,
            ModuleTitle = module?.Title ?? model.ModuleTitle
        };

        return View("QuizResult", result);
    }

    private async Task<IActionResult> RetakeQuizView(QuizSubmissionViewModel model)
    {
        if (string.IsNullOrEmpty(model.ModuleTitle) && model.ModuleId > 0)
        {
            var module = await _context.LearningModules.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == model.ModuleId);
            model.ModuleTitle = module?.Title ?? "Quiz";
        }

        return View("TakeQuiz", model);
    }
}
