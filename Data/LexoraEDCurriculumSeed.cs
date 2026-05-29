using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Data;

public static class LexoraEDCurriculumSeed
{
    private const string AnchorModuleTitle = "Basic Greetings and Introductions";

    public static void EnsureCurriculum(LexoraEDContext context)
    {
        UpgradeLegacyDifficultyLevels(context);

        var catalog = LexoraEDCurriculumCatalog.GetAllModules();
        var anchorMissing = !context.LearningModules.Any(m => m.Title == AnchorModuleTitle);
        var hasMissingCatalogModules = catalog.Any(c =>
            !context.LearningModules.Any(m => m.Title == c.Title));

        if (!anchorMissing && !hasMissingCatalogModules)
            return;

        SeedMissingModules(context);
    }

    public static void UpgradeLegacyDifficultyLevels(LexoraEDContext context)
    {
        context.Database.ExecuteSqlRaw(
            "UPDATE LearningModules SET DifficultyLevel = N'Beginner' WHERE DifficultyLevel = N'Easy'");
        context.Database.ExecuteSqlRaw(
            "UPDATE LearningModules SET DifficultyLevel = N'Intermediate' WHERE DifficultyLevel = N'Medium'");
        context.Database.ExecuteSqlRaw(
            "UPDATE LearningModules SET DifficultyLevel = N'Advanced' WHERE DifficultyLevel = N'Hard'");

        context.Database.ExecuteSqlRaw(
            "UPDATE LearningProgresses SET CurrentLevel = N'Beginner' WHERE CurrentLevel = N'Easy'");
        context.Database.ExecuteSqlRaw(
            "UPDATE LearningProgresses SET CurrentLevel = N'Intermediate' WHERE CurrentLevel = N'Medium'");
        context.Database.ExecuteSqlRaw(
            "UPDATE LearningProgresses SET CurrentLevel = N'Advanced' WHERE CurrentLevel = N'Hard'");
    }

    private static void SeedMissingModules(LexoraEDContext context)
    {
        var catalog = LexoraEDCurriculumCatalog.GetAllModules();
        var existingTitles = context.LearningModules
            .AsNoTracking()
            .Select(m => m.Title)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var addedModules = new List<(LearningModule Module, CurriculumModuleDefinition Definition)>();

        foreach (var definition in catalog)
        {
            if (existingTitles.Contains(definition.Title))
                continue;

            var module = new LearningModule
            {
                Title = definition.Title,
                Description = definition.Description,
                Content = definition.Content,
                Category = definition.Category,
                DifficultyLevel = definition.DifficultyLevel,
                SortOrder = definition.SortOrder
            };

            context.LearningModules.Add(module);
            addedModules.Add((module, definition));
            existingTitles.Add(definition.Title);
        }

        if (addedModules.Count == 0)
            return;

        context.SaveChanges();

        foreach (var (module, definition) in addedModules)
        {
            if (definition.PrerequisiteSortOrder is int prereqOrder)
            {
                var prerequisite = context.LearningModules
                    .FirstOrDefault(m =>
                        m.DifficultyLevel == definition.DifficultyLevel &&
                        m.SortOrder == prereqOrder);

                if (prerequisite != null)
                {
                    module.PrerequisiteModuleId = prerequisite.Id;
                }
            }

            var quizSet = new QuizSet { LearningModuleId = module.Id };
            context.QuizSets.Add(quizSet);
            context.SaveChanges();

            var quizItems = definition.Questions.Select(q => new QuizItem
            {
                QuizSetId = quizSet.Id,
                QuestionType = q.QuestionType,
                QuestionText = q.QuestionText,
                ChoiceA = q.ChoiceA,
                ChoiceB = q.ChoiceB,
                ChoiceC = q.ChoiceC,
                ChoiceD = q.ChoiceD,
                CorrectAnswer = q.CorrectAnswer,
                Explanation = q.Explanation
            });

            context.QuizItems.AddRange(quizItems);
        }

        context.SaveChanges();
    }
}
