
using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH.Hospital;

public interface IHospitalPermissionService
{
    Task<ServiceResult<IEnumerable<string>>> GetAllPermissionServiceAsync();
}
