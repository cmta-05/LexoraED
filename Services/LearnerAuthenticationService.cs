using LexoraED.Data;
using LexoraED.Models;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class LearnerAuthenticationService
{
    private readonly LexoraEDContext _context;

    public LearnerAuthenticationService(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string? ErrorMessage, Learner? Learner)> RegisterLearnerAsync(
        string fullName,
        string username,
        string password,
        LearnerRole role)
    {
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return (false, "All fields are required.", null);
        }

        var exists = await _context.Learners.AnyAsync(l => l.Username == username);
        if (exists)
        {
            return (false, "Username is already taken.", null);
        }

        var learner = new Learner
        {
            FullName = fullName.Trim(),
            Username = username.Trim(),
            PasswordHash = LearnerCredentialHasher.HashPassword(password),
            Role = role
        };

        _context.Learners.Add(learner);
        await _context.SaveChangesAsync();

        if (role == LearnerRole.Student)
        {
            _context.LearningProgresses.Add(new LearningProgress
            {
                LearnerId = learner.Id,
                CurrentLevel = DifficultyLevel.Beginner,
                CompletedModulesCount = 0
            });
            await _context.SaveChangesAsync();
        }

        if (role == LearnerRole.Teacher)
        {
            _context.TeacherProfiles.Add(new TeacherProfile { LearnerId = learner.Id });
            await _context.SaveChangesAsync();
        }

        return (true, null, learner);
    }

    public async Task<(bool Success, string? ErrorMessage, Learner? Learner)> AuthenticateLearnerAsync(
        string username,
        string password)
    {
        var normalizedUsername = username.Trim();
        var learner = await _context.Learners
            .FirstOrDefaultAsync(l => l.Username.ToLower() == normalizedUsername.ToLower());

        if (learner == null || !learner.IsActive)
        {
            return (false, "Invalid username or password.", null);
        }

        if (!LearnerCredentialHasher.VerifyPassword(password, learner.PasswordHash))
        {
            return (false, "Invalid username or password.", null);
        }

        return (true, null, learner);
    }
}
