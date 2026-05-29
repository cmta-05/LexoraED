using LexoraED.Models;
using LexoraED.Services;

namespace LexoraED.Data;

public static class LexoraEDSeedData
{
    public static void Initialize(LexoraEDContext context)
    {
        if (context.Learners.Any())
            return;

        var admin = new Learner
        {
            FullName = "LexoraED Administrator",
            Username = "admin",
            PasswordHash = LearnerCredentialHasher.HashPassword("Admin@123"),
            Role = LearnerRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var teacher = new Learner
        {
            FullName = "Sample Teacher",
            Username = "teacher",
            PasswordHash = LearnerCredentialHasher.HashPassword("Teacher@123"),
            Role = LearnerRole.Teacher,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var student = new Learner
        {
            FullName = "Sample Student",
            Username = "student",
            PasswordHash = LearnerCredentialHasher.HashPassword("Student@123"),
            Role = LearnerRole.Student,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Learners.AddRange(admin, teacher, student);
        context.SaveChanges();

        context.TeacherProfiles.Add(new TeacherProfile { LearnerId = teacher.Id });

        context.LearningProgresses.Add(new LearningProgress
        {
            LearnerId = student.Id,
            CurrentLevel = DifficultyLevel.Beginner,
            CompletedModulesCount = 0
        });

        var grammarEasy = new LearningModule
        {
            Title = "Present Simple Tense",
            Description = "Learn how to form and use the present simple tense in everyday English.",
            Content = "The present simple is used for habits, facts, and routines. Subject + base verb (add -s for he/she/it).",
            Category = ModuleCategory.Grammar,
            DifficultyLevel = DifficultyLevel.Beginner
        };

        var vocabularyEasy = new LearningModule
        {
            Title = "Everyday Greetings",
            Description = "Essential greeting words and phrases for daily conversation.",
            Content = "Hello, Hi, Good morning, Good afternoon, Good evening, How are you?",
            Category = ModuleCategory.Vocabulary,
            DifficultyLevel = DifficultyLevel.Beginner
        };

        var readingMedium = new LearningModule
        {
            Title = "Short Story Comprehension",
            Description = "Read a short passage and identify main ideas and details.",
            Content = "Read carefully and look for who, what, when, where, and why in the text.",
            Category = ModuleCategory.Reading,
            DifficultyLevel = DifficultyLevel.Intermediate
        };

        var grammarHard = new LearningModule
        {
            Title = "Conditional Sentences",
            Description = "Master first, second, and third conditional structures.",
            Content = "Zero: If + present, present. First: If + present, will. Second: If + past, would. Third: If + past perfect, would have.",
            Category = ModuleCategory.Grammar,
            DifficultyLevel = DifficultyLevel.Advanced
        };

        context.LearningModules.AddRange(grammarEasy, vocabularyEasy, readingMedium, grammarHard);
        context.SaveChanges();

        var grammarQuiz = new QuizSet { LearningModuleId = grammarEasy.Id };
        var vocabularyQuiz = new QuizSet { LearningModuleId = vocabularyEasy.Id };

        context.QuizSets.AddRange(grammarQuiz, vocabularyQuiz);
        context.SaveChanges();

        context.QuizItems.AddRange(
            new QuizItem
            {
                QuizSetId = grammarQuiz.Id,
                QuestionText = "Which sentence uses the present simple correctly?",
                ChoiceA = "She go to school every day.",
                ChoiceB = "She goes to school every day.",
                ChoiceC = "She going to school every day.",
                ChoiceD = "She gone to school every day.",
                CorrectAnswer = "B"
            },
            new QuizItem
            {
                QuizSetId = grammarQuiz.Id,
                QuestionText = "The present simple is often used for:",
                ChoiceA = "Actions happening right now",
                ChoiceB = "Completed actions in the past",
                ChoiceC = "Habits and routines",
                ChoiceD = "Future plans with going to",
                CorrectAnswer = "C"
            },
            new QuizItem
            {
                QuizSetId = vocabularyQuiz.Id,
                QuestionText = "What is a polite morning greeting?",
                ChoiceA = "Good night",
                ChoiceB = "Good morning",
                ChoiceC = "See you later",
                ChoiceD = "Take care",
                CorrectAnswer = "B"
            },
            new QuizItem
            {
                QuizSetId = vocabularyQuiz.Id,
                QuestionText = "\"How are you?\" is typically answered with:",
                ChoiceA = "I am fine, thank you.",
                ChoiceB = "I am eating lunch.",
                ChoiceC = "At the library.",
                ChoiceD = "Because I studied.",
                CorrectAnswer = "A"
            });

        context.SaveChanges();
    }
}
