using LexoraED.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Data;

public class LexoraEDContext : IdentityDbContext<ApplicationUser>
{
    public LexoraEDContext(DbContextOptions<LexoraEDContext> options)
        : base(options)
    {
    }

    public DbSet<LearningModule> LearningModules => Set<LearningModule>();
    public DbSet<QuizSet> QuizSets => Set<QuizSet>();
    public DbSet<QuizItem> QuizItems => Set<QuizItem>();
    public DbSet<LearningAttempt> LearningAttempts => Set<LearningAttempt>();
    public DbSet<LearningProgress> LearningProgresses => Set<LearningProgress>();
    public DbSet<ModuleProgress> ModuleProgresses => Set<ModuleProgress>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<AchievementBadge> AchievementBadges => Set<AchievementBadge>();
    public DbSet<LearnerAchievement> LearnerAchievements => Set<LearnerAchievement>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FullName).HasMaxLength(200).IsRequired();
            entity.Property(u => u.AccountStatus).HasConversion<string>().HasMaxLength(20);
        });

        builder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.StudentIdNumber).IsUnique();
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.StudentIdNumber).HasMaxLength(50).IsRequired();
            entity.HasOne(e => e.User)
                .WithOne(u => u.StudentProfile)
                .HasForeignKey<StudentProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TeacherProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TeacherIdNumber).IsUnique();
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.TeacherIdNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SchoolName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Department).HasMaxLength(100).IsRequired();
            entity.Property(e => e.SubjectSpecialization).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ContactNumber).HasMaxLength(30).IsRequired();
            entity.HasOne(e => e.User)
                .WithOne(u => u.TeacherProfile)
                .HasForeignKey<TeacherProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<LearningModule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.DifficultyLevel).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(e => e.PrerequisiteModule)
                .WithMany()
                .HasForeignKey(e => e.PrerequisiteModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<QuizSet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.LearningModule)
                .WithMany(m => m.QuizSets)
                .HasForeignKey(e => e.LearningModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<QuizItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuestionText).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.CorrectAnswer).HasMaxLength(500).IsRequired();
            entity.Property(e => e.QuestionType).HasConversion<string>().HasMaxLength(30);
            entity.HasOne(e => e.QuizSet)
                .WithMany(q => q.QuizItems)
                .HasForeignKey(e => e.QuizSetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<LearningAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.LearningAttempts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.QuizSet)
                .WithMany(q => q.LearningAttempts)
                .HasForeignKey(e => e.QuizSetId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<LearningProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.CurrentLevel).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(e => e.User)
                .WithOne(u => u.LearningProgress)
                .HasForeignKey<LearningProgress>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ModuleProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.LearningModuleId }).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany(u => u.ModuleProgresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.LearningModule)
                .WithMany(m => m.ModuleProgresses)
                .HasForeignKey(e => e.LearningModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<AchievementBadge>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Title).HasMaxLength(120).IsRequired();
        });

        builder.Entity<LearnerAchievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.AchievementBadgeId }).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany(u => u.LearnerAchievements)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.AchievementBadge)
                .WithMany(b => b.LearnerAchievements)
                .HasForeignKey(e => e.AchievementBadgeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActivityType).HasMaxLength(80).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany(u => u.ActivityLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
