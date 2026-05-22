using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Services.SEIH.User;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebAPI.Controllers.SEIH.User;

[ApiController]
[Route("seih/identity/user")]
public class SignInUserSEIHController : BaseController
{
    private readonly IUsersSignIn _service;

    public SignInUserSEIHController(IUsersSignIn service)
    {
        _service = service;
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("loginseih", Name = "UserSEIHLogin")]
    public async Task<ActionResult<UserSignInResponse>> LoginUserAsync(UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            // Extract error messages from ModelState
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Return BadRequest with error messages
            return BadRequest(new { Errors = errorMessages });
        }
        var result = await _service.SingInUserAsync(model);

        if (result.IsError)
        {
            return BadRequest(result);
        }

        var response = result.Result;

        Response.Cookies.Append("SessionId", response.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
        });


        Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        UserSignInResponse res = new UserSignInResponse()
        {
            FirstName = response.FirstName,
            LastName = response.LastName,
            IsNewPasswordRequired = response.IsNewPasswordRequired,
            Initial = response.Initial,
            UserRoles = response.UserRoles,
            Email = response.Email,
            RefreshToken = response.RefreshToken,
            Token = response.Token,
        };
        
        return Ok(new ServiceResult<UserSignInResponse>(res));
    }

}
