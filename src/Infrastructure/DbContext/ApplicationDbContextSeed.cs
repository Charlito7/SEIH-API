using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Infrastructure;

public class ApplicationDbContextSeed
{
    internal static void Initialize(AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        dbContext.Database.EnsureCreated();

        if (dbContext.Set<UserEntity>().Any()) { return; }


        var roles = Enum.GetValues(typeof(UserRoleEnums)).Cast<UserRoleEnums>();

        foreach (var role in roles)
        {
            var roleStore = new RoleStore<IdentityRole>(dbContext);

            if (!dbContext.Roles.Any(r => r.Name == role.ToString()))
            {
                roleStore.CreateAsync(new IdentityRole(role.ToString()));
            }
        }
        dbContext.SaveChanges();
    }

    internal static async void AddAdminRole(IServiceProvider services, string email)
    {
        UserManager<UserEntity> _userManager = services.GetService<UserManager<UserEntity>>()!;
        UserEntity user = await _userManager.FindByEmailAsync(email)!;

        await _userManager.AddToRoleAsync(user, UserRoleEnums.Admin.ToString());
    }

}
