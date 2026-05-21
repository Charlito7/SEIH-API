using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.User
{
    public class DoesUserBelongToRole : IDoesUserBelongToRole
    {
        private readonly AppDbContext _context;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        public DoesUserBelongToRole(AppDbContext context, IUserManager userManager,
            IRoleManager roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ServiceResult<bool>> DoesUserBelongToRoleAsync(string role, string email)
        {
  
            if(!await _roleManager.IsRoleExists(role))
            {
                return new ServiceResult<bool>(false);
            }
            var user = await _context.Set<UserEntity>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            
            if(await _userManager.IsInRoleASync(user!, role))
            {
                return new ServiceResult<bool>(true);
            }

            return new ServiceResult<bool>(false);
            
        }
    }
}
