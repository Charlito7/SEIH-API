using Core.Application.Interface.Repository.SEIH;
using Core.Domain.Entities;
using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;
using Infrastructure.Services.SEIH.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository.SEIH.User;

public class RoleRepository : IRolesRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public RoleRepository(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<bool> AssignRoles(Guid? userId, Guid? roleIds)
    {
        if (userId == null || roleIds == null)
            return false;

        try
        {  
            UsersRoleEntity newUserRoles = new UsersRoleEntity
            {
                UserId = userId.Value,
                RoleId = roleIds.Value,
                Created = DateTime.UtcNow,
                IsDeleted = false
            };

            await _context.UsersRole.AddAsync(newUserRoles);
            await _context.SaveChangesAsync(); 

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning roles to user {UserId}", userId);
            return false;
        }
    }


    public async Task<RolesEntity?> GetHospitalRole(Guid hospitalId, string roleName)
    {
        try
        {
            return await _context.Rolesv2
                .FirstOrDefaultAsync(r =>
                    r.HospitalId == hospitalId &&
                    r.Name!.ToLower() == roleName.ToLower() &&
                    r.IsDeleted == false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching role '{RoleName}' for hospital {HospitalId}", roleName, hospitalId);
            throw;
        }
    }

    public async Task<UsersRoleEntity?> GetUserRole(Guid userId, Guid roleId)
    {
        try
        {
            return await _context.UsersRole
                .FirstOrDefaultAsync(r =>
                    r.UserId == userId &&
                    r.RoleId == roleId &&
                    r.IsDeleted == false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching role '{RoleId}' for user {userId}", roleId, userId);
            throw;
        }
    }
}
