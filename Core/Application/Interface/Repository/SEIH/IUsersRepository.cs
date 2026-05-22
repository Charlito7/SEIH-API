using Core.Domain.Entity.SEIH;
using Core.Domain.Procedures.SEIH;

namespace Core.Application.Interface.Repository.SEIH;

public interface IUsersRepository
{
    Task<bool> CreateUserAsync(UsersEntity user);
    Task<bool> UpdateUserAsync(UsersEntity user);
    Task<bool> DeleteUserAsync(string userId);
    Task<UsersEntity?> GetUserByIdAsync(Guid userId);
    Task<UsersEntity?> GetUserByEmailAsync(string email);
    Task<IEnumerable<GetUserRolesResponse>> GetUserRolesAsync(Guid? userId);
    Task<IEnumerable<GetUserRolesWithPermissionResponse>> GetUserRolesWithPermissionAsync(Guid? userId);
    Task<IEnumerable<GetUserListWithRolesResponse>> GetAllHospitalUsersWithRolesAsync(Guid hospitalId);

}
