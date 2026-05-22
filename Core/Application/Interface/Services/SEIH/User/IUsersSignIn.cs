using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Request;
using Core.Application.Model.Response;

namespace Core.Application.Interface.Services.SEIH.User;

public interface IUsersSignIn
{
    Task<ServiceResult<UserSignInResponse>> SingInUserAsync(UserLoginModel model);
}
