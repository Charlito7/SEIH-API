using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Application.Interfaces.Services.User;

public interface ICreateUserService
{
    Task<ServiceResult<bool>> CreateUserAsync(CreateUserModel dataModel);
    Task<ServiceResult<bool>> CreateUserAsync(CreateUserModel dataModel, ExternalLoginInfo info);
}
