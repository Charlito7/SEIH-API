using Core.Application.Commons.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services.User;

public interface IAdminRemoveRoles
{
    Task<ServiceResult<bool>> RemoveUserRolesAsync(string email, string[] roles);
}
