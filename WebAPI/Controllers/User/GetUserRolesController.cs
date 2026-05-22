using Application.Interfaces.Repositories.User;
using Core.Application.Interface.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebAPI.Controllers.User
{
    [Route("user/roles")]
    public class GetUserRolesController : AuthorizeBaseController
    {
        private readonly IUserManager _userManager;
        public GetUserRolesController(ITokenServices token, IUserManager userManager) : base(token)
        {
        }

        [AuthorizeRoles]
        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> GetUserRolesAsync()
        {
            var roles = User.Claims
             .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
             .Select(c => c.Value).ToList();
            return Ok(roles);

        }
    }
}
