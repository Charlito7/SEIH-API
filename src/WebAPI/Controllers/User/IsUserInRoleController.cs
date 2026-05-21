using Application.Interfaces.Repositories.User;
using Core.Application.Interface.Token;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Route("user/role")]
    public class IsUserInRoleController : AuthorizeBaseController
    {
        private readonly IDoesUserBelongToRole _service;
        private readonly IUserManager _userManager;

        public IsUserInRoleController(IDoesUserBelongToRole service, IUserManager userManager, ITokenServices tokenServices)
            : base(tokenServices)
        {
            _service = service;
            _userManager = userManager;
        }

        [AuthorizeRoles]
        [HttpPost]
        [Route("check")]
        public async Task<IActionResult> IsUserInRoleAsync(UserRoleCheck req)
        {
            bool hasRole = req.roles.Any(role => User.IsInRole(role));

            if (hasRole)
            {
                return Ok(hasRole);
            }
            return BadRequest(hasRole);
            /* if (req.roles.Count() <= 0)
             {
                 return BadRequest("Bad model");
             }
             var userEmail = this.User.FindFirst(ClaimTypes.Email)?.Value;
             if (userEmail == null || string.IsNullOrEmpty(userEmail))
                 return BadRequest();

             var user = await _userManager.FindByEmailAsync(userEmail);
             if (user == null)
                 return Unauthorized();

             var result = await _service.DoesUserBelongToRoleAsync(model.role, userEmail);
             if(result.Result == false)
             {
                 return Unauthorized();
             } */

        }
    }
}
