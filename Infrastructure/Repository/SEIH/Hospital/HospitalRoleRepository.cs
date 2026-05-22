using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;
using Infrastructure.Services.SEIH.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repository.SEIH.Hospital;

public class HospitalRoleRepository : IHospitalRoleRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public HospitalRoleRepository(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<RolePermissionEntity?> GetPermissionByNameAsync(string permissionName)
    {
        throw new NotImplementedException();
    }

    public async Task<RolesEntity?> GetRoleByNameAsync(string roleName, Guid hospitalId)
    {
        if (roleName == null || hospitalId.ToString() == null)
        {
            _logger.LogWarning("Attempted to get a null role.");
            return null;
        }
        var role = await _context.Rolesv2.Where(c => c.Name == roleName && c.HospitalId == hospitalId).FirstOrDefaultAsync();

        return role;
    }

    public async Task<IEnumerable<string>> GetRoleListAsyncRepository(string hospitalId)
    {
        // Fix: Use '==' for comparison, and convert hospitalId to Guid for comparison with RolesEntity.HospitalId
        if (!Guid.TryParse(hospitalId, out Guid hospitalGuid))
        {
            _logger.LogWarning("Invalid hospitalId format.");
            return Enumerable.Empty<string>();
        }

        return await _context.Rolesv2
            .Where(p => p.HospitalId == hospitalGuid)
            .Select(p => p.Name!)
            .ToListAsync();
    }

    public async Task<bool> HospitalAddPermissionToRoleUserAsync(RolePermissionEntity rolePermission)
    {
        if (rolePermission == null)
        {
            _logger.LogWarning("Attempted to create a null user.");
            return true;
        }

        try
        {
            // Génère un nouvel ID si nécessaire
            if (rolePermission.Id == null || rolePermission.Id == Guid.Empty)
                rolePermission.Id = Guid.NewGuid();

            rolePermission.Created = DateTime.UtcNow;
            rolePermission.IsDeleted = false;

            await _context.RolesPermission.AddAsync(rolePermission);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating user.");
            return false;
        }
    }

    public async Task<bool> HospitalCreateRoleAsync(RolesEntity role)
    {
        if (role == null)
        {
            _logger.LogWarning("Attempted to create a null user.");
            return true;
        }

        try
        {
            // Génère un nouvel ID si nécessaire
            if (role.Id == null || role.Id == Guid.Empty)
                role.Id = Guid.NewGuid();

            role.Created = DateTime.UtcNow;
            role.IsDeleted = false;

            await _context.Rolesv2.AddAsync(role);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating user.");
            return false;
        }
    }

    public Task<IEnumerable<RolesEntity>> HospitalGetAllRolesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RolePermissionEntity>> HospitalGetAllRoleWithPermissionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> HospitalUpdatePermissionToRoleUserAsync(RolePermissionEntity rolePermission)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HospitalUpdateRoleAsync(RolesEntity role)
    {
        throw new NotImplementedException();
    }
}
