using Core.Application.Commons.ServiceResult;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services.User;

public interface IAdminAssignRoles
{
    Task<ServiceResult<bool>> AssignRolesToUser(string userEmail, string[] roles);
}
