using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebApi.Controllers.Admin
{
    [Route("admin/roles/")]
    public class AddRolesToUserController : BaseController
    {
        private readonly IAdminAssignRoles _service;
        private readonly IDoesUserBelongToRole _userRoleService;

        public AddRolesToUserController(IAdminAssignRoles service, IDoesUserBelongToRole userRoleService)
        {
            _service = service;
            _userRoleService = userRoleService;
        }

        [HttpPost]
        [AuthorizeRoles]
        [Route("add", Name = "AddRolesToUser")]
        public async Task<IActionResult> AdminAssignRolesAsync([FromBody] string data)
        {
            //TODO: grab claims from token. 
            //verify if Admin Role. 
            if (Environment.GetEnvironmentVariable(InfrastructureConstants.ASP_NET_CORE_ENVIRONMENT_NAME)
                                                                        == InfrastructureConstants.DEV_ENVIRONMENT_NAME && data == "test")
            {
                var myModel = new AddRolesToUserModel
                {
                    Email = "mpalaniappan@gmail.com",
                    Roles = new string[] { "User", "Admin"}
                };

                data = JsonConvert.SerializeObject(myModel);
            }

            if (string.IsNullOrWhiteSpace(data))
                return BadRequest();

            var userEmail = this.User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null || string.IsNullOrEmpty(userEmail))
                return Unauthorized();
            var model = JsonConvert.DeserializeObject<AddRolesToUserModel>(data!);

            var roleResult = await _userRoleService.DoesUserBelongToRoleAsync("Admin", userEmail);

            if(roleResult.Result == false)
            {
                return Unauthorized();
            }

            var result = await _service.AssignRolesToUser(model!.Email!, model.Roles!);

            if (result.IsError)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}
