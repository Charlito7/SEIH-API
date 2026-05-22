using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH.Hospital;

public interface IHospitalUserRepository
{
    Task<bool> HospitalCreateUserAsync(UsersEntity user);
    Task<bool> HospitalAddRoleToUserAsync(UsersRoleEntity userRole);
    Task<bool> HospitalUpdateUserPasswordAsync(UsersEntity user);
    Task<bool> HospitalUpdateUserAsync(UsersEntity user);
}
