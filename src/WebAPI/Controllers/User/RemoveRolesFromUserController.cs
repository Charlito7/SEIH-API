using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Domain.Enums;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebApi.Controllers.User
{
    [Route("admin/roles/")]
    public class RemoveRolesFromUserController : BaseController
    {
        private readonly IAdminRemoveRoles _service;
        private readonly IDoesUserBelongToRole _roleService;

        public RemoveRolesFromUserController(IAdminRemoveRoles service, IDoesUserBelongToRole roleService)
        {
            _service = service;
            _roleService = roleService;
        }

        [HttpPost]
        [AuthorizeRoles]
        [Route("remove", Name ="RemoveRolesFromUser")]
        public async Task<IActionResult> AdminAssignRolesAsync([FromBody] string data)
        {
            var userEmail = this.User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null || string.IsNullOrEmpty(userEmail))
                return Unauthorized();
            
            var isAdminResult = await _roleService.DoesUserBelongToRoleAsync(UserRoleEnums.Admin.ToString(), userEmail);

            if (isAdminResult.Result != true)
            {
                return Unauthorized();
            }  

            if (string.IsNullOrWhiteSpace(data))
                return BadRequest();


            var model = JsonConvert.DeserializeObject<AddRolesToUserModel>(data!);

            var result = await _service.RemoveUserRolesAsync(model!.Email!, model.Roles!);

            if (result.IsError)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}
