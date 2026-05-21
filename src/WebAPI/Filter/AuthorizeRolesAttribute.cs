using Core.Application.Interface.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace WebApi.Filters
{
    public class AuthorizeRolesAttribute : TypeFilterAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base(typeof(AuthorizeRolesFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthorizeRolesFilter : IAuthorizationFilter
    {
        private readonly string[] _roles;
        private readonly ITokenServices _tokenServices;

        public AuthorizeRolesFilter(string[] roles, ITokenServices tokenServices)
        {
            _roles = roles;
            _tokenServices = tokenServices;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Read the token from the cookie
            var accessToken = context.HttpContext.Request.Cookies["SessionId"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                // Validate the token and get the principal
                var principal = _tokenServices.GetPrincipalFromToken(accessToken);
                if (principal == null)
                {
                    context.Result = new UnauthorizedObjectResult("Invalid Token");
                    return;
                }

                // Check if password change is required
                var isNewPasswordRequiredClaim = principal.FindFirst("IsNewPasswordRequired")?.Value;
                if (bool.TryParse(isNewPasswordRequiredClaim, out var isNewPasswordRequired) && isNewPasswordRequired)
                {
                    context.Result = new UnauthorizedObjectResult(new
                    {
                        Message = "Password change required",
                        Code = "PASSWORD_CHANGE_REQUIRED"
                    });
                    return;
                }

                // Set the user principal
                context.HttpContext.User = principal;

                // Check if the user has any of the required roles
                if (_roles.Any() && !_roles.Any(role => principal.IsInRole(role)))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Token not found");
                return;
            }
        }
    }
}