using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Services.Emails;
using Core.Application.Interface.Token;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Core.Domain.Entities;
using Infrastructure.Constants;
using Infrastructure.Security;
using System.Diagnostics;
using System.Net;

namespace Infrastructure.Services.User
{
    public class SignInUsers : ISignInUser
    {
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;
        private readonly ITokenServices _tokenServices;

        public SignInUsers(ISignInManager signInManager, IUserManager userManager,
            ITokenServices tokenServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenServices = tokenServices;
        }

        public async Task<ServiceResult<UserSignInResponse>> SingInAsync(UserLoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return new ServiceResult<UserSignInResponse>(HttpStatusCode.BadRequest);
            }
            UserEntity? user = null;

           user = await _userManager.FindByEmailAsync(model.UserName!);
            if (user == null)
            {
                return new ServiceResult<UserSignInResponse>(HttpStatusCode.BadRequest);
            }

            
            var result = MyPasswordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (!result)
            {
                return new ServiceResult<UserSignInResponse>(HttpStatusCode.Unauthorized);
            }

            
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
  
            var response = new UserSignInResponse
            {
                Token = _tokenServices
                .BuildToken(Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET),
                Environment.GetEnvironmentVariable(EnvFileConstants.ISSUER),
                Environment.GetEnvironmentVariable(EnvFileConstants.AUDIENCE), user, userRoles),

                RefreshToken = _tokenServices.GenerateRefreshToken(),
                IsNewPasswordRequired = user.IsNewPasswordRequired,
                UserRoles = userRoles,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Initial = (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName))
        ? $"{char.ToUpper(user.FirstName[0])}{char.ToUpper(user.LastName[0])}"
        : string.Empty
            };
         

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            
            await _userManager.UpdateAsync(user);

            return new ServiceResult<UserSignInResponse>(response);
        }
    }
}
