using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Data;

public class LexoraEDContext : DbContext
{
    public LexoraEDContext(DbContextOptions<LexoraEDContext> options)
        : base(options)
    {
    }

    public DbSet<Learner> Learners => Set<Learner>();
    public DbSet<LearningModule> LearningModules => Set<LearningModule>();
    public DbSet<QuizSet> QuizSets => Set<QuizSet>();
    public DbSet<QuizItem> QuizItems => Set<QuizItem>();
    public DbSet<LearningAttempt> LearningAttempts => Set<LearningAttempt>();
    public DbSet<LearningProgress> LearningProgresses => Set<LearningProgress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.FullName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<LearningModule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.DifficultyLevel).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<QuizSet>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.LearningModule)
                .WithMany(m => m.QuizSets)
                .HasForeignKey(e => e.LearningModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuizItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuestionText).HasMaxLength(500).IsRequired();
            entity.Property(e => e.CorrectAnswer).HasMaxLength(1).IsRequired();

            entity.HasOne(e => e.QuizSet)
                .WithMany(q => q.QuizItems)
                .HasForeignKey(e => e.QuizSetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LearningAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Learner)
                .WithMany(l => l.LearningAttempts)
                .HasForeignKey(e => e.LearnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.QuizSet)
                .WithMany(q => q.LearningAttempts)
                .HasForeignKey(e => e.QuizSetId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LearningProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CurrentLevel).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(e => e.Learner)
                .WithOne(l => l.LearningProgress)
                .HasForeignKey<LearningProgress>(e => e.LearnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
