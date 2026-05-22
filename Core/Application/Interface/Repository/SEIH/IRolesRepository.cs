using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH;

public interface IRolesRepository
{
    Task<RolesEntity?> GetHospitalRole(Guid hospitalId, string roleName);
    Task<UsersRoleEntity?> GetUserRole(Guid userId, Guid roleId);
    Task<bool> AssignRoles(Guid? userId, Guid? roleId);
}
