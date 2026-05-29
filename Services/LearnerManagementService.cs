using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class LearnerManagementService
{
    private readonly LexoraEDContext _context;

    public LearnerManagementService(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task<LearnerManagementIndexViewModel> GetLearnersAsync(
        string? search,
        LearnerRole? roleFilter,
        int page = 1,
        int pageSize = 9)
    {
        var query = _context.Learners.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(l =>
                l.Username.ToLower().Contains(term) ||
                l.FullName.ToLower().Contains(term));
        }

        if (roleFilter.HasValue)
            query = query.Where(l => l.Role == roleFilter.Value);

        var total = await query.CountAsync();
        var learners = await query
            .OrderBy(l => l.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LearnerCardViewModel
            {
                Id = l.Id,
                FullName = l.FullName,
                Username = l.Username,
                Role = l.Role,
                IsActive = l.IsActive,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return new LearnerManagementIndexViewModel
        {
            Learners = learners,
            Search = search,
            RoleFilter = roleFilter,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public async Task<Learner?> GetLearnerAsync(int id)
        => await _context.Learners.Include(l => l.LearningProgress).FirstOrDefaultAsync(l => l.Id == id);

    public async Task<(bool Success, string? Error)> CreateLearnerAsync(LearnerFormViewModel model)
    {
        if (await _context.Learners.AnyAsync(l => l.Username == model.Username.Trim()))
            return (false, "Username already exists.");

        if (string.IsNullOrWhiteSpace(model.Password))
            return (false, "Password is required for new users.");

        var learner = new Learner
        {
            FullName = model.FullName.Trim(),
            Username = model.Username.Trim(),
            PasswordHash = LearnerCredentialHasher.HashPassword(model.Password!),
            Role = model.Role,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Learners.Add(learner);
        await _context.SaveChangesAsync();
        await EnsureRoleSetupAsync(learner);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateLearnerAsync(LearnerFormViewModel model)
    {
        var learner = await _context.Learners.FindAsync(model.Id);
        if (learner == null)
            return (false, "User not found.");

        if (await _context.Learners.AnyAsync(l => l.Username == model.Username.Trim() && l.Id != model.Id))
            return (false, "Username already exists.");

        learner.FullName = model.FullName.Trim();
        learner.Username = model.Username.Trim();
        learner.Role = model.Role;
        learner.IsActive = model.IsActive;

        if (!string.IsNullOrWhiteSpace(model.Password))
            learner.PasswordHash = LearnerCredentialHasher.HashPassword(model.Password!);

        await _context.SaveChangesAsync();
        await EnsureRoleSetupAsync(learner);
        return (true, null);
    }

    public async Task<bool> DeleteLearnerAsync(int id)
    {
        var learner = await _context.Learners.FindAsync(id);
        if (learner == null)
            return false;

        _context.Learners.Remove(learner);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetPasswordAsync(int id, string newPassword)
    {
        var learner = await _context.Learners.FindAsync(id);
        if (learner == null || string.IsNullOrWhiteSpace(newPassword))
            return false;

        learner.PasswordHash = LearnerCredentialHasher.HashPassword(newPassword);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task EnsureRoleSetupAsync(Learner learner)
    {
        if (learner.Role == LearnerRole.Student &&
            !await _context.LearningProgresses.AnyAsync(p => p.LearnerId == learner.Id))
        {
            _context.LearningProgresses.Add(new LearningProgress
            {
                LearnerId = learner.Id,
                CurrentLevel = DifficultyLevel.Beginner,
                CompletedModulesCount = 0
            });
            await _context.SaveChangesAsync();
        }

        if (learner.Role == LearnerRole.Teacher &&
            !await _context.TeacherProfiles.AnyAsync(t => t.LearnerId == learner.Id))
        {
            _context.TeacherProfiles.Add(new TeacherProfile { LearnerId = learner.Id });
            await _context.SaveChangesAsync();
        }
    }
}
