using BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BettingSystem.Filters
{
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!role.ToPermissions().Includes(Permissions.ManageUsers | Permissions.ViewAnalytics))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}