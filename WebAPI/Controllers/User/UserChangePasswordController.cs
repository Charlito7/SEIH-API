using Core.Application.Interface.Token;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebAPI.Controllers.User;

[ApiController]
[Route("identity/user/password")]
public class UserChangePasswordController : BaseController
{
    private readonly IUpdateUserPassword _updateUserPassword;
    public UserChangePasswordController(IUpdateUserPassword updateUserPassword)
    {
        _updateUserPassword = updateUserPassword;
    }

    [HttpPost]
    [Route("change")]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] UpdateUserPasswordModel request)
    {
       var result = await _updateUserPassword.UpdateUserPassowrdAsync(request);

        if (result.IsOk)
        {
            return Ok(true);
        }
        return BadRequest(false);

    }

}
