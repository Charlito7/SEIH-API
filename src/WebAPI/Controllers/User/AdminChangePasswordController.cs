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
public class AdminChangePasswordController : AuthorizeBaseController
{
    private readonly IUpdateUserPassword _updateUserPassword;
    public AdminChangePasswordController(IUpdateUserPassword updateUserPassword,ITokenServices token) : base(token)
    {
        _updateUserPassword = updateUserPassword;
    }


    [AuthorizeRoles("Manager")]
    [HttpPost]
    [Route("manager/change")]
    public async Task<IActionResult> ChangeUserPasswordByManagerAsync([FromBody] UpdateUserPasswordByManagerModel request)
    {
        var result = await _updateUserPassword.UpdateUserPassowrdByManagerAsync(User, request);

        if (result.IsOk)
        {
            return Ok(true);
        }
        return BadRequest(false);

    }


}
