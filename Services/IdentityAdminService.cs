using LexoraED.Data;
using LexoraED.Models;
using LexoraED.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LexoraED.Services;

public class IdentityAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly LexoraEDContext _context;

    public IdentityAdminService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        LexoraEDContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<UserManagementIndexViewModel> GetUsersAsync(string? search, string? roleFilter, AccountStatus? statusFilter, int page = 1, int pageSize = 9)
    {
        var query = _context.Users.AsNoTracking()
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.LearningProgress)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(u =>
                u.UserName!.ToLower().Contains(term) ||
                u.FullName.ToLower().Contains(term) ||
                (u.Email != null && u.Email.ToLower().Contains(term)));
        }

        var users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
        var cards = new List<UserCardViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "—";
            if (!string.IsNullOrEmpty(roleFilter) && role != roleFilter)
                continue;
            if (statusFilter.HasValue && user.AccountStatus != statusFilter.Value)
                continue;

            cards.Add(new UserCardViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Username = user.UserName ?? "",
                Role = role,
                AccountStatus = user.AccountStatus,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                StudentIdNumber = user.StudentProfile?.StudentIdNumber,
                TeacherIdNumber = user.TeacherProfile?.TeacherIdNumber,
                SchoolName = user.TeacherProfile?.SchoolName
            });
        }

        var total = cards.Count;
        var paged = cards.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new UserManagementIndexViewModel
        {
            Users = paged,
            Search = search,
            RoleFilter = roleFilter,
            StatusFilter = statusFilter,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize))
        };
    }

    public async Task<UserDetailsViewModel?> GetUserDetailsAsync(string userId)
    {
        var user = await _context.Users
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.LearningProgress)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        return new UserDetailsViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? "",
            Username = user.UserName ?? "",
            Role = roles.FirstOrDefault() ?? "—",
            AccountStatus = user.AccountStatus,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            StudentProfile = user.StudentProfile,
            TeacherProfile = user.TeacherProfile,
            LearningProgress = user.LearningProgress
        };
    }

    public async Task<bool> ApproveTeacherAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        user.AccountStatus = AccountStatus.Approved;
        user.IsActive = true;
        return (await _userManager.UpdateAsync(user)).Succeeded;
    }

    public async Task<bool> RejectTeacherAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        user.AccountStatus = AccountStatus.Rejected;
        user.IsActive = false;
        return (await _userManager.UpdateAsync(user)).Succeeded;
    }

    public async Task<bool> SetActiveAsync(string userId, bool isActive)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        user.IsActive = isActive;
        return (await _userManager.UpdateAsync(user)).Succeeded;
    }

    public async Task<(bool Success, string? Error)> CreateAdminAsync(AdminCreateViewModel model)
    {
        if (await _userManager.FindByNameAsync(model.Username) != null)
            return (false, "Username already exists.");
        if (await _userManager.FindByEmailAsync(model.Email) != null)
            return (false, "Email already exists.");

        var user = new ApplicationUser
        {
            UserName = model.Username.Trim(),
            Email = model.Email.Trim(),
            EmailConfirmed = true,
            FullName = model.FullName.Trim(),
            AccountStatus = AccountStatus.Approved,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return (false, string.Join(" ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, LexoraRoles.Admin);
        return (true, null);
    }
}
