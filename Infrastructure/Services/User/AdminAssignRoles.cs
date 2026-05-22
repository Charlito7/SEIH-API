
using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.User
{
    public class AdminAssignRoles : IAdminAssignRoles
    {
        private readonly IRoleManager _roleManager;
        private readonly IUserManager _userManager;

        public AdminAssignRoles(IRoleManager roleManager, IUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ServiceResult<bool>> AssignRolesToUser(string userEmail, string[] roles)
        {
            if(string.IsNullOrWhiteSpace(userEmail))
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            if(user == null)
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.NotFound);
            }

            var myroles = Enum.GetValues(typeof(UserRoleEnums)).Cast<UserRoleEnums>();

            foreach (var role in roles)
            {
                if (!await _roleManager.IsRoleExists(role))
                {
                    await _roleManager.CreateRoleAsync(role);
                }
            }

            foreach(var role in roles)
            {
                if (await _roleManager.IsRoleExists(role))
                {
                    if(!await _userManager.IsInRoleASync(user,role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                    
                }
            }

            return new ServiceResult<bool>(true);

        }
    }
}
