using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories.User;

public interface IRoleManager
{
    Task<bool> IsRoleExists(string roleName);
    Task<IdentityResult> CreateRoleAsync(string roleName);
    
}
