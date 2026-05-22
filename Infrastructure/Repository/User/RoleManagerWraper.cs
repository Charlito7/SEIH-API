using Application.Interfaces.Repositories.User;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.User;

public class RoleManagerWrapper : IRoleManager
{
    private readonly RoleManager<UserRoleEntity> _roleManager;

    public RoleManagerWrapper(RoleManager<UserRoleEntity> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<IdentityResult> CreateRoleAsync(string roleName)
    {
        var role = new UserRoleEntity { Name = roleName, NormalizedName = roleName.ToUpper() };
        return await _roleManager.CreateAsync(role);
    }

    public async Task<bool> IsRoleExists(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }
}
