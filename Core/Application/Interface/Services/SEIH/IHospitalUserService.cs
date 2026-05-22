
using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using Core.Application.Model.Request;
using Core.Domain.Procedures.SEIH;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH;

public interface IHospitalUserService
{
    Task<ServiceResult<bool>> HospitalCreateUserServiceAsync(ClaimsPrincipal claim, CreateUserModel dataModel);
    Task<ServiceResult<bool>> HospitalAddRoleToUserServiceAsync(ClaimsPrincipal claim, AddRolesToUserDTO dataModel);
    Task<ServiceResult<bool>> HospitalUpdateUserPasswordServiceAsync(ClaimsPrincipal claim, ChangePasswordModel dataModel);
    Task<ServiceResult<bool>> HospitalUpdateUserServiceAsync(ClaimsPrincipal claim, CreateUserModel dataModel);
    Task<ServiceResult<bool>> HospitalUpdateUserPasswordByManagerServiceAsync(ClaimsPrincipal claim, ChangePasswordByManagerModel dataModel);
    Task<ServiceResult<IEnumerable<GetUserListWithRolesResponse>>> HospitalGetUsersListWithRolesAsync(ClaimsPrincipal claim);
}
