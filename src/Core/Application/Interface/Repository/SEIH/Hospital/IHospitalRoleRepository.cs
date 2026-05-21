using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH.Hospital;

public interface IHospitalRoleRepository
{
    Task<bool> HospitalCreateRoleAsync(RolesEntity role);
    Task<bool> HospitalAddPermissionToRoleUserAsync(RolePermissionEntity rolePermission);
    Task<bool> HospitalUpdatePermissionToRoleUserAsync(RolePermissionEntity rolePermission);
    Task<bool> HospitalUpdateRoleAsync(RolesEntity role);
    Task<IEnumerable<RolesEntity>> HospitalGetAllRolesAsync();
    Task<IEnumerable<RolePermissionEntity>> HospitalGetAllRoleWithPermissionsAsync();
    Task<RolesEntity?> GetRoleByNameAsync(string roleName, Guid hospitalId);
    Task<RolePermissionEntity?> GetPermissionByNameAsync(string permissionName);
    Task<IEnumerable<string>> GetRoleListAsyncRepository(string hospitalId);
}
