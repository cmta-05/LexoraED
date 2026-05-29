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

    public async Task<IActionResult> QuizCenter()
    {
        var learnerId = HttpContext.Session.GetInt32(LearnerSessionKeys.LearnerId)!.Value;
        return View(await _learningPathPresentation.BuildQuizCenterAsync(learnerId));
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

        var model = new QuizSubmissionViewModel
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

        if (model.Answers.Any(a => string.IsNullOrWhiteSpace(a.SelectedAnswer)))
        {
            ModelState.AddModelError(string.Empty, "Please answer all questions before submitting.");
            return await RetakeQuizView(model);
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
                CorrectAnswer = FormatCorrectDisplay(item),
                IsCorrect = isCorrect,
                Explanation = item.Explanation,
                QuestionType = item.QuestionType
            });
        }

        var score = (int)Math.Round((double)correctCount / quizItems.Count * 100);

        var evaluation = await _adaptiveLearningPathService.EvaluateLearnerPerformanceAsync(
            learnerId, model.QuizSetId, score);

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

    private static string FormatCorrectDisplay(QuizItem item) => item.QuestionType switch
    {
        QuizQuestionType.MultipleChoice => $"Choice {item.CorrectAnswer}",
        QuizQuestionType.TrueFalse => item.CorrectAnswer,
        _ => item.CorrectAnswer
    };

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
