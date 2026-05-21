using Core.Application.Interface.Repository.SEIH;
using Core.Domain.Entity.SEIH;
using Core.Domain.Procedures.SEIH;
using Infrastructure.Services.SEIH.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz.Logging;
using QuestPDF.Helpers;

namespace Infrastructure.Repository.SEIH.User;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UsersRepository(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<bool> CreateUserAsync(UsersEntity user)
    {
        if (user == null)
        {
            _logger.LogWarning("Attempted to create a null user.");
            return true;
        }

        try
        {
            // Génère un nouvel ID si nécessaire
            if (user.Id == null || user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();

            user.Created = DateTime.UtcNow;
            user.IsDeleted = false;

            await _context.User.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating user.");
            return false;
        }
    } 

    public Task<bool> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<GetUserListWithRolesResponse>> GetAllHospitalUsersWithRolesAsync(Guid hospitalId)
    {
        return await _context.GetUserListWithRoles
                    .FromSqlRaw("CALL SEIH_GetUserListWithRoles({0})", hospitalId)
                    .ToListAsync();
    }



    public async Task<UsersEntity?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _context.User
                             .Where(u => u.Email == email && (u.IsDeleted == null || u.IsDeleted == false))
                             .FirstOrDefaultAsync();

    }

    public async  Task<UsersEntity?> GetUserByIdAsync(Guid userId)
    {
        return await _context.User
                             .Where(u => u.Id == userId && (u.IsDeleted == null || u.IsDeleted == false))
                             .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GetUserRolesResponse>> GetUserRolesAsync(Guid? userId)
    {
        return await _context.GetUserRoles
.FromSqlRaw("CALL SEIH_GetUserRoles({0})", userId)
.ToListAsync();
    }

    public async Task<IEnumerable<GetUserRolesWithPermissionResponse>> GetUserRolesWithPermissionAsync(Guid? userId)
    {

            return await _context.GetUserRolesWithPermission
.FromSqlRaw("CALL SEIH_GetUserRolesWithPermissions({0})", userId!)
.ToListAsync();


    }

    public async Task<bool> UpdateUserAsync(UsersEntity user)
    {


        try
        {
            _context.User.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            // Optionally log the error
            _logger?.LogError(ex, "Failed to update user.");
            return false;
        }
    }
}
