using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Data;

public static class LexoraEDMinimalCurriculumSeed
{
    private static readonly string[] KeepTitles =
    [
        "Present Simple Tense",
        "Colors and Descriptions",
        "Verb Tenses",
        "Adjectives",
        "Critical Reading",
        "Grammar Analysis"
    ];

    public static void Apply(LexoraEDContext context)
    {
        var toRemove = context.LearningModules
            .Where(m => !KeepTitles.Contains(m.Title))
            .ToList();

        if (toRemove.Count > 0)
        {
            context.LearningModules.RemoveRange(toRemove);
            context.SaveChanges();
        }

        EnsureModule(context, "Present Simple Tense", ModuleCategory.Grammar, DifficultyLevel.Beginner, 1,
            "Master the present simple for habits, facts, and routines.",
            BuildLesson("Present Simple Tense", "Use base verbs and add -s for he/she/it."));

        EnsureModule(context, "Colors and Descriptions", ModuleCategory.Vocabulary, DifficultyLevel.Beginner, 2,
            "Learn color words and how to describe objects.",
            BuildLesson("Colors and Descriptions", "Red, blue, green — pair colors with nouns in short phrases."));

        EnsureModule(context, "Verb Tenses", ModuleCategory.Grammar, DifficultyLevel.Intermediate, 1,
            "Understand past, present, and future time references.",
            BuildLesson("Verb Tenses", "Past simple, present continuous, and future with will/going to."));

        EnsureModule(context, "Adjectives", ModuleCategory.Vocabulary, DifficultyLevel.Intermediate, 2,
            "Use adjectives to describe people, places, and ideas.",
            BuildLesson("Adjectives", "Order adjectives and comparatives for clearer descriptions."));

        EnsureModule(context, "Critical Reading", ModuleCategory.Reading, DifficultyLevel.Advanced, 1,
            "Analyze texts for purpose, tone, and evidence.",
            BuildLesson("Critical Reading", "Identify claims, support, and author bias in academic passages."));

        EnsureModule(context, "Grammar Analysis", ModuleCategory.Grammar, DifficultyLevel.Advanced, 2,
            "Break down complex sentences and clause structures.",
            BuildLesson("Grammar Analysis", "Analyze subordinate clauses, modifiers, and sentence variety."));

        context.SaveChanges();
    }

    private static void EnsureModule(
        LexoraEDContext context,
        string title,
        ModuleCategory category,
        DifficultyLevel level,
        int sortOrder,
        string description,
        string content)
    {
        var module = context.LearningModules
            .Include(m => m.QuizSets)
            .ThenInclude(q => q.QuizItems)
            .FirstOrDefault(m => m.Title == title);

        if (module == null)
        {
            module = new LearningModule
            {
                Title = title,
                Description = description,
                Content = content,
                Category = category,
                DifficultyLevel = level,
                SortOrder = sortOrder
            };
            context.LearningModules.Add(module);
            context.SaveChanges();

            var quizSet = new QuizSet { LearningModuleId = module.Id };
            context.QuizSets.Add(quizSet);
            context.SaveChanges();
            AddDefaultQuizItems(context, quizSet.Id, title);
            return;
        }

        module.Description = description;
        module.Content = content;
        module.Category = category;
        module.DifficultyLevel = level;
        module.SortOrder = sortOrder;

        if (!module.QuizSets.Any())
        {
            var qs = new QuizSet { LearningModuleId = module.Id };
            context.QuizSets.Add(qs);
            context.SaveChanges();
            AddDefaultQuizItems(context, qs.Id, title);
        }
        else if (!module.QuizSets.SelectMany(q => q.QuizItems).Any())
        {
            AddDefaultQuizItems(context, module.QuizSets.First().Id, title);
        }
    }

    private static string BuildLesson(string title, string summary) =>
        $"## Introduction\n{title} is a core microlesson in LexoraED.\n\n## Learning Objectives\n- Understand key concepts\n- Apply them in short exercises\n\n## Main Discussion\n{summary}\n\n## Examples\nPractice with short sentences daily.\n\n## Summary\nReview this lesson before taking the quiz.";

    private static void AddDefaultQuizItems(LexoraEDContext context, int quizSetId, string topic)
    {
        for (var i = 1; i <= 10; i++)
        {
            context.QuizItems.Add(new QuizItem
            {
                QuizSetId = quizSetId,
                QuestionType = QuizQuestionType.MultipleChoice,
                QuestionText = $"{topic} — Question {i}: Choose the best answer.",
                ChoiceA = "Option A",
                ChoiceB = "Option B (correct)",
                ChoiceC = "Option C",
                ChoiceD = "Option D",
                CorrectAnswer = "B",
                Explanation = "Review the lesson section on main discussion."
            });
        }

        context.QuizItems.Add(new QuizItem
        {
            QuizSetId = quizSetId,
            QuestionType = QuizQuestionType.TrueFalse,
            QuestionText = $"{topic} is important for English fluency.",
            ChoiceA = "True",
            ChoiceB = "False",
            CorrectAnswer = "True"
        });

        context.QuizItems.Add(new QuizItem
        {
            QuizSetId = quizSetId,
            QuestionType = QuizQuestionType.Identification,
            QuestionText = "The adaptive LMS is called ____.",
            CorrectAnswer = "LexoraED"
        });

        context.SaveChanges();
    }
}
