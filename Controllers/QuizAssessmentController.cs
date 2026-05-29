using System.Security.Claims;
using LexoraED.Data;
using LexoraED.Models;
using LexoraED.Services;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Controllers;

[Authorize(Roles = LexoraRoles.Student)]
public class QuizAssessmentController : Controller
{
    private readonly LexoraEDContext _context;
    private readonly AdaptiveLearningPathService _adaptiveLearningPathService;
    private readonly LearningPathPresentationService _learningPathPresentation;
    private readonly QuizScoringService _quizScoringService;

    public QuizAssessmentController(
        LexoraEDContext context,
        AdaptiveLearningPathService adaptiveLearningPathService,
        LearningPathPresentationService learningPathPresentation,
        QuizScoringService quizScoringService)
    {
        _context = context;
        _adaptiveLearningPathService = adaptiveLearningPathService;
        _learningPathPresentation = learningPathPresentation;
        _quizScoringService = quizScoringService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> QuizCenter()
    {
        return View(await _learningPathPresentation.BuildQuizCenterAsync(UserId));
    }

    public async Task<IActionResult> TakeQuiz(int moduleId)
    {
        var studyModule = await _learningPathPresentation.BuildStudyModuleAsync(UserId, moduleId);
        if (studyModule == null) return NotFound();
        if (studyModule.IsLocked)
        {
            TempData["LexoraMessage"] = "This quiz is locked until you advance your learning path.";
            return RedirectToAction(nameof(QuizCenter));
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

        return View(new QuizSubmissionViewModel
        {
            QuizSetId = quizSet.Id,
            ModuleId = moduleId,
            ModuleTitle = quizSet.LearningModule.Title,
            Answers = quizSet.QuizItems.OrderBy(i => i.Id).Select(item => new QuizAnswerViewModel
            {
                QuizItemId = item.Id,
                QuestionType = item.QuestionType,
                QuestionText = item.QuestionText,
                ChoiceA = item.ChoiceA,
                ChoiceB = item.ChoiceB,
                ChoiceC = item.ChoiceC,
                ChoiceD = item.ChoiceD
            }).ToList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitQuiz(QuizSubmissionViewModel model)
    {
        var quizItems = await _context.QuizItems.AsNoTracking()
            .Where(q => q.QuizSetId == model.QuizSetId).ToListAsync();

        if (!quizItems.Any()) return NotFound();

        if (model.Answers == null || model.Answers.Count != quizItems.Count ||
            model.Answers.Any(a => string.IsNullOrWhiteSpace(a.SelectedAnswer)))
        {
            ModelState.AddModelError(string.Empty, "Please answer all questions before submitting.");
            return View("TakeQuiz", model);
        }

        var reviews = new List<QuizAnswerReviewViewModel>();
        var correctCount = 0;
        foreach (var answer in model.Answers)
        {
            var item = quizItems.First(q => q.Id == answer.QuizItemId);
            var isCorrect = _quizScoringService.IsAnswerCorrect(item, answer.SelectedAnswer);
            if (isCorrect) correctCount++;
            reviews.Add(new QuizAnswerReviewViewModel
            {
                QuestionText = item.QuestionText,
                SelectedAnswer = answer.SelectedAnswer,
                CorrectAnswer = item.CorrectAnswer,
                IsCorrect = isCorrect,
                Explanation = item.Explanation,
                QuestionType = item.QuestionType
            });
        }

        var score = (int)Math.Round((double)correctCount / quizItems.Count * 100);
        var evaluation = await _adaptiveLearningPathService.EvaluateLearnerPerformanceAsync(UserId, model.QuizSetId, score);
        var (tier, headline) = QuizFeedbackHelper.GetFeedback(score);

        var module = await _context.QuizSets.AsNoTracking()
            .Where(q => q.Id == model.QuizSetId)
            .Select(q => new { q.LearningModuleId, q.LearningModule.Title })
            .FirstOrDefaultAsync();

        TempData["QuizSuccess"] = true;

        return View("QuizResult", new QuizResultViewModel
        {
            Score = evaluation.Score,
            GuidanceMessage = evaluation.GuidanceMessage,
            FeedbackTier = tier,
            FeedbackHeadline = headline,
            RecommendedLevel = evaluation.RecommendedLevel,
            CurrentLevel = evaluation.CurrentLevel,
            CompletedModulesCount = evaluation.CompletedModulesCount,
            ExperiencePoints = evaluation.ExperiencePoints,
            CurrentStreak = evaluation.CurrentStreak,
            ModuleId = module?.LearningModuleId ?? model.ModuleId,
            ModuleTitle = module?.Title ?? model.ModuleTitle,
            AnswerReviews = reviews
        });
    }
}
