using LexoraED.Models;
using LexoraED.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LexoraED.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireLearnerRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly LearnerRole[] _allowedRoles;

    public RequireLearnerRoleAttribute(params LearnerRole[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session;
        var learnerId = session.GetInt32(LearnerSessionKeys.LearnerId);
        var roleString = session.GetString(LearnerSessionKeys.LearnerRole);

        if (!learnerId.HasValue || string.IsNullOrEmpty(roleString))
        {
            context.Result = new RedirectToActionResult("Login", "LearnerAccess", null);
            return;
        }

        if (!Enum.TryParse<LearnerRole>(roleString, out var role) || !_allowedRoles.Contains(role))
        {
            context.Result = new RedirectToActionResult("Login", "LearnerAccess", null);
        }
    }
}
