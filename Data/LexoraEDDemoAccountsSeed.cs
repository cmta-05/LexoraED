using LexoraED.Models;
using LexoraED.Services;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Data;

/// <summary>
/// Ensures demo accounts exist and can sign in (fixes IsActive=false on migrated users).
/// </summary>
public static class LexoraEDDemoAccountsSeed
{
    private static readonly DemoAccount[] DemoAccounts =
    [
        new("admin", "Admin@123", "LexoraED Administrator", LearnerRole.Admin),
        new("teacher", "Teacher@123", "Sample Teacher", LearnerRole.Teacher),
        new("student", "Student@123", "Sample Student", LearnerRole.Student)
    ];

    public static void EnsureDemoAccounts(LexoraEDContext context)
    {
        // Migration LexoraEDUpgradeV2 set IsActive default to false — reactivate everyone.
        context.Database.ExecuteSqlRaw("UPDATE Learners SET IsActive = 1 WHERE IsActive = 0");

        foreach (var demo in DemoAccounts)
        {
            var learner = context.Learners
                .Include(l => l.LearningProgress)
                .FirstOrDefault(l => l.Username.ToLower() == demo.Username.ToLower());

            if (learner == null)
            {
                learner = new Learner
                {
                    FullName = demo.FullName,
                    Username = demo.Username,
                    PasswordHash = LearnerCredentialHasher.HashPassword(demo.Password),
                    Role = demo.Role,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Learners.Add(learner);
                context.SaveChanges();
            }
            else
            {
                learner.IsActive = true;
                learner.PasswordHash = LearnerCredentialHasher.HashPassword(demo.Password);
                learner.Role = demo.Role;
                learner.FullName = demo.FullName;
            }

            EnsureRoleData(context, learner);
        }

        context.SaveChanges();
    }

    private static void EnsureRoleData(LexoraEDContext context, Learner learner)
    {
        if (learner.Role == LearnerRole.Student &&
            !context.LearningProgresses.Any(p => p.LearnerId == learner.Id))
        {
            context.LearningProgresses.Add(new LearningProgress
            {
                LearnerId = learner.Id,
                CurrentLevel = DifficultyLevel.Beginner,
                CompletedModulesCount = 0
            });
        }

        if (learner.Role == LearnerRole.Teacher &&
            !context.TeacherProfiles.Any(t => t.LearnerId == learner.Id))
        {
            context.TeacherProfiles.Add(new TeacherProfile { LearnerId = learner.Id });
        }
    }

    private sealed record DemoAccount(string Username, string Password, string FullName, LearnerRole Role);
}
