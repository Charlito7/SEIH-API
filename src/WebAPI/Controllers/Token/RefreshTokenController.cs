using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Token;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Infrastructure.Constants;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.Token
{
    [Route("user/token")]
    public class RefreshTokenController : BaseController
    {
        private readonly IUserManager _userManager;
        private readonly ITokenServices _services;

        public RefreshTokenController(IUserManager userManager, ITokenServices tokenServices)
        {
            _userManager= userManager;
            _services= tokenServices;
        }

        [AllowAnonymous]
        [HttpPost("refresh", Name ="RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenModel model)
        {
            //var deserializedData = Deserializer<TokenModel>.DeserializeObject(request.Data!);
            var accessToken = model.Token;  //deserializedData?.Token;
            var refreshToken = model.RefreshToken; //deserializedData?.RefreshToken;

            var principal = _services.GetPrincipalFromExpiredToken(accessToken!,
                                                            Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET));
            if(principal == null)
            {
                return Unauthorized("Invalid Client Request");
            }
            var email = principal.FindFirst(ClaimTypes.Email)!.Value!;
            //var userName = principal!.Identity!.Name;

            var user = await _userManager.FindByEmailAsync(email!);

            if (user == null || user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                return Unauthorized("Invalid Client Request");


            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {

                    var isTokenValidated = _services
                        .ValidateTokenWithoutExpiryTime(Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET),
                    Environment.GetEnvironmentVariable(EnvFileConstants.ISSUER), Environment.GetEnvironmentVariable(EnvFileConstants.AUDIENCE),
                                                accessToken);
                    if (isTokenValidated)
                    {
                        IList<string> userRoles = await _userManager.GetRolesAsync(user);
                        var newGeneratedToken = _services
                        .BuildToken(Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET),
                    Environment.GetEnvironmentVariable(EnvFileConstants.ISSUER), Environment.GetEnvironmentVariable(EnvFileConstants.AUDIENCE),
                                                user, userRoles);
                        HttpContext.Session.SetString("Token", newGeneratedToken);

                        var newRefreshToken = _services.GenerateRefreshToken();
                        user.RefreshToken = newRefreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                        await _userManager.UpdateAsync(user);
                        var response = new ExternalLoginResponse
                        {
                            Token = newGeneratedToken,
                            RefreshToken = newRefreshToken,
                        };
                        var result = new ServiceResult<ExternalLoginResponse>(response);
                        return Ok(result);

                    }

                }
                catch
                {
                    return Unauthorized();
                }

            }
            return Unauthorized();
        }

    }
}

