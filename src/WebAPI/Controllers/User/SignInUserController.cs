using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.User;

[ApiController]
[Route("identity/user")]
public class SignInUserSEIHController : BaseController
{
    private readonly ISignInUser _service;

    public SignInUserSEIHController(ISignInUser service)
    {
        _service = service;
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("login", Name = "UserLogin")]
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
        var result = await _service.SingInAsync(model);

        if (result.IsError)
        {
            return BadRequest(result);
        }

        var response = result.Result;

        Response.Cookies.Append("SessionId", response.Token!, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
        });


        Response.Cookies.Append("RefreshToken", response.RefreshToken!, new CookieOptions
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
            Email = response.Email
        };
        
        return Ok(new ServiceResult<UserSignInResponse>(res));
    }

}
