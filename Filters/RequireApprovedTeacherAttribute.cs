using LexoraED.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LexoraED.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireApprovedTeacherAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.GetUserAsync(context.HttpContext.User);

        if (user == null || !user.IsActive)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (await userManager.IsInRoleAsync(user, LexoraRoles.Admin))
            return;

        if (user.AccountStatus == AccountStatus.Pending)
        {
            context.Result = new RedirectToActionResult("PendingApproval", "Account", null);
            return;
        }

        if (user.AccountStatus == AccountStatus.Rejected)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
        }
    }
}
