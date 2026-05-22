using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.User
{
    public class AdminRemoveRoles : IAdminRemoveRoles
    {
        private readonly IRoleManager _roleManager;
        private readonly IUserManager _userManager;

        public AdminRemoveRoles(IRoleManager roleManager, IUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ServiceResult<bool>> RemoveUserRolesAsync(string userEmail, string[] roles)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.NotFound);
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            
            foreach (var role in roles)
            {
                var roleToRemove = userRoles.FirstOrDefault(role => role.Equals(role, StringComparison.InvariantCultureIgnoreCase));
                if (roleToRemove == null)
                {
                    return new ServiceResult<bool>(System.Net.HttpStatusCode.NotFound);
                }
                var result = await _userManager.RemoveFromRolesAsync(user, role);
                if (!result.Succeeded)
                {
                    return new ServiceResult<bool>(System.Net.HttpStatusCode.InternalServerError);
                }
            }

            return new ServiceResult<bool>(true);
        }
    }
}
