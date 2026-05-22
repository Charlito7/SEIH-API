using Core.Application.Interface.Token;
using Infrastructure.Constants;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Controllers.Base
{
    public class AuthorizeBaseController : Controller
    {
        private readonly ITokenServices _tokenServices;

        public AuthorizeBaseController(ITokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(a => a.Value!.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .ToList();
                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            // Read the token from the cookie
            var accessToken = context.HttpContext.Request.Cookies["SessionId"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                bool isTokenValidated = _tokenServices
                    .ValidateTokenWithExpiryTime(
                        Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET),
                        Environment.GetEnvironmentVariable(EnvFileConstants.ISSUER),
                        Environment.GetEnvironmentVariable(EnvFileConstants.AUDIENCE),
                        accessToken);

                if (!isTokenValidated)
                {
                    context.Result = new UnauthorizedObjectResult("Invalid Token");
                    return;
                }

                // Set the user principal if the token is valid
                var principal = _tokenServices.GetPrincipalFromToken(accessToken);
                context.HttpContext.User = principal;
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Token not found");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}