using Core.Application.Interface.Token;
using Infrastructure.Constants;

using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebApi.Controllers.Token
{
    [Route("user/token")]
    public class ValidateToken : Controller
    {
        [HttpPost]
        [Route("validate")]
        [AuthorizeRoles]
        public async Task<IActionResult> ValidateTokenAsync()
        {
            return Ok();
        }
    }
}
