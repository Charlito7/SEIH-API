
using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH.Hospital;

public interface IHospitalRoleService
{
    Task<ServiceResult<bool>> HospitalCreateRoleServiceAsync(ClaimsPrincipal claim, HospitalRoleDto dataModel);
    Task<ServiceResult<bool>> HospitalAddPermissionToRoleServiceAsync(ClaimsPrincipal claim, HospitalRolePermissionDto dataModel);
    Task<ServiceResult<IEnumerable<string>>> GetAllRoleServiceAsync(ClaimsPrincipal claim);
}
