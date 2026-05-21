using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Services.Emails;
using Core.Application.Interface.Services.SEIH.User;
using Core.Application.Interface.Token;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Core.Domain.Entities;
using Core.Domain.Entity.SEIH;
using Core.Domain.Procedures.SEIH;
using Infrastructure.Constants;
using Infrastructure.Security;
using System.Diagnostics;
using System.Net;

namespace Infrastructure.Services.SEIH.User;

public class SignInUsers : IUsersSignIn
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenServices _tokenServices;

    public SignInUsers(
        IUsersRepository usersRepository,
        ITokenServices tokenServices)
    {
        _usersRepository = usersRepository;
        _tokenServices = tokenServices;
    }

    public async Task<ServiceResult<UserSignInResponse>> SingInUserAsync(UserLoginModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserName))
        {
            return new ServiceResult<UserSignInResponse>(HttpStatusCode.BadRequest);
        }
        UsersEntity? user = null;

       user = await _usersRepository.GetUserByEmailAsync(model.UserName);
        if (user == null)
        {
            return new ServiceResult<UserSignInResponse>(HttpStatusCode.BadRequest);
        }

        
        var result = MyPasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, model.Password!);


        if (!result)
        {
            return new ServiceResult<UserSignInResponse>(HttpStatusCode.Unauthorized);
        }

        
        IEnumerable<GetUserRolesResponse> userRoles = await _usersRepository.GetUserRolesAsync(user.Id);
        IList<string> roleNames = userRoles
    .Select(r => r.RoleName)
    .ToList();


        var response = new UserSignInResponse
        {
            Token = _tokenServices
            .BuildToken2(Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET)!,
            Environment.GetEnvironmentVariable(EnvFileConstants.ISSUER)!,
            Environment.GetEnvironmentVariable(EnvFileConstants.AUDIENCE)!, user, userRoles),

            RefreshToken = _tokenServices.GenerateRefreshToken(),
            IsNewPasswordRequired = user.IsNewPasswordRequired,
            UserRoles = roleNames,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Initial = (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName))
    ? $"{char.ToUpper(user.FirstName[0])}{char.ToUpper(user.LastName[0])}"
    : string.Empty
        };
     

        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        
        await _usersRepository.UpdateUserAsync(user);

        return new ServiceResult<UserSignInResponse>(response);
    }
}
