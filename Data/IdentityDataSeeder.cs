using LexoraED.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Data;

public static class IdentityDataSeeder
{
    public const string DefaultAdminEmail = "admin@lexoraed.local";
    public const string DefaultAdminUsername = "admin";
    public const string DefaultAdminPassword = "Admin@123";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var context = services.GetRequiredService<LexoraEDContext>();

        foreach (var role in LexoraRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        await EnsureAdminAsync(userManager);
        await EnsureDemoStudentAsync(userManager, context);
        await EnsureDemoTeacherAsync(userManager, context);
        await EnsureAchievementBadgesAsync(context);
    }

    private static async Task EnsureAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var admin = await userManager.FindByNameAsync(DefaultAdminUsername);
        if (admin != null)
        {
            admin.AccountStatus = AccountStatus.Approved;
            admin.IsActive = true;
            await userManager.UpdateAsync(admin);
            if (!await userManager.IsInRoleAsync(admin, LexoraRoles.Admin))
                await userManager.AddToRoleAsync(admin, LexoraRoles.Admin);
            return;
        }

        admin = new ApplicationUser
        {
            UserName = DefaultAdminUsername,
            Email = DefaultAdminEmail,
            EmailConfirmed = true,
            FullName = "LexoraED Administrator",
            AccountStatus = AccountStatus.Approved,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        if ((await userManager.CreateAsync(admin, DefaultAdminPassword)).Succeeded)
            await userManager.AddToRoleAsync(admin, LexoraRoles.Admin);
    }

    private static async Task EnsureDemoStudentAsync(UserManager<ApplicationUser> userManager, LexoraEDContext context)
    {
        var user = await userManager.FindByNameAsync("student");
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "student",
                Email = "student@lexoraed.local",
                EmailConfirmed = true,
                FullName = "Sample Student",
                AccountStatus = AccountStatus.Approved,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            if (!(await userManager.CreateAsync(user, "Student@123")).Succeeded)
                return;
            await userManager.AddToRoleAsync(user, LexoraRoles.Student);
        }

        if (!await context.StudentProfiles.AnyAsync(s => s.UserId == user.Id))
        {
            context.StudentProfiles.Add(new StudentProfile
            {
                UserId = user.Id,
                StudentIdNumber = "STU-0001",
                LearningPreference = "Balanced microlearning"
            });
        }

        if (!await context.LearningProgresses.AnyAsync(p => p.UserId == user.Id))
        {
            context.LearningProgresses.Add(new LearningProgress
            {
                UserId = user.Id,
                CurrentLevel = DifficultyLevel.Beginner,
                CompletedModulesCount = 0
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task EnsureDemoTeacherAsync(UserManager<ApplicationUser> userManager, LexoraEDContext context)
    {
        var user = await userManager.FindByNameAsync("teacher");
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "teacher",
                Email = "teacher@lexoraed.local",
                EmailConfirmed = true,
                FullName = "Sample Teacher",
                AccountStatus = AccountStatus.Approved,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            if (!(await userManager.CreateAsync(user, "Teacher@123")).Succeeded)
                return;
            await userManager.AddToRoleAsync(user, LexoraRoles.Teacher);
        }
        else
        {
            user.AccountStatus = AccountStatus.Approved;
            user.IsActive = true;
            await userManager.UpdateAsync(user);
        }

        if (!await context.TeacherProfiles.AnyAsync(t => t.UserId == user.Id))
        {
            context.TeacherProfiles.Add(new TeacherProfile
            {
                UserId = user.Id,
                TeacherIdNumber = "TCH-0001",
                SchoolName = "LexoraED Academy",
                Department = "English Education",
                SubjectSpecialization = "ESL Instruction",
                ContactNumber = "+63-900-000-0001"
            });
            await context.SaveChangesAsync();
        }
    }

    private static async Task EnsureAchievementBadgesAsync(LexoraEDContext context)
    {
        if (await context.AchievementBadges.AnyAsync())
            return;

        context.AchievementBadges.AddRange(
            new AchievementBadge { Code = "GRAMMAR_BEGINNER", Title = "Grammar Beginner", Description = "Strong grammar performance.", PointsAwarded = 50, IconClass = "badge-grammar" },
            new AchievementBadge { Code = "VOCAB_MASTER", Title = "Vocabulary Master", Description = "Vocabulary excellence.", PointsAwarded = 75, IconClass = "badge-vocab" },
            new AchievementBadge { Code = "QUIZ_STREAK_5", Title = "Quiz Streak x5", Description = "Five quiz attempts.", PointsAwarded = 40, IconClass = "badge-streak" },
            new AchievementBadge { Code = "PERFECT_SCORE", Title = "Perfect Score Award", Description = "100% on a quiz.", PointsAwarded = 100, IconClass = "badge-perfect" },
            new AchievementBadge { Code = "MODULE_MASTER", Title = "Module Master", Description = "Mastered a module.", PointsAwarded = 60, IconClass = "badge-master" });
        await context.SaveChangesAsync();
    }
}
