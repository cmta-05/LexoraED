using LexoraED.Data;
using LexoraED.Models;

namespace LexoraED.Services;

public class ActivityLogService
{
    private readonly LexoraEDContext _context;

    public ActivityLogService(LexoraEDContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string userId, string activityType, string description)
    {
        _context.ActivityLogs.Add(new ActivityLog
        {
            UserId = userId,
            ActivityType = activityType,
            Description = description,
            LoggedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }
}
