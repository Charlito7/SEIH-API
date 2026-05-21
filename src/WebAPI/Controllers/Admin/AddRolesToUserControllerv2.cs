using Application.Interfaces.Repositories.User;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.Admin
{
    [Route("admin/roles/")]
    public class AddRolesToUserControllerv2 : BaseController
    {
        private readonly IAdminAssignRoles _service;
        private readonly IDoesUserBelongToRole _userRoleService;
        private readonly IUserManager _userManager;

        public AddRolesToUserControllerv2(IAdminAssignRoles service, IDoesUserBelongToRole userRoleService,
            IUserManager userManager)
        {
            _service = service;
            _userRoleService = userRoleService;
            _userManager = userManager;
        }

        [HttpPost]
        //[AuthorizeRoles]
        [Route("locally/add", Name = "LocallyAddRolesToUser")]
        public async Task<IActionResult> AdminLocallyAssignRolesAsync([FromBody] AddRolesToUserModel request)
        {
            
            if ( string.IsNullOrEmpty(request.AdminEmail))
                return Unauthorized();
            
            var roleResult = await _userRoleService.DoesUserBelongToRoleAsync("Admin", request.AdminEmail);

            if(roleResult.Result == false)
            {
                return Unauthorized();
            }

            var result = await _service.AssignRolesToUser(request!.Email!, request.Roles!);

            if (result.IsError)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}
