using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using Core.Application.Model.Request;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH.User;

public interface IUserService
{
    Task<ServiceResult<bool>> SEIH_CreateUserAsync(ClaimsPrincipal claim,CreateUserModel dataModel);
    Task<ServiceResult<bool>> SEIH_AddRolesToUserAsync(ClaimsPrincipal claim,AddRolesToUserDto dataModel);
}
