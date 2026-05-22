using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.User
{
    public class UpdateUserPassword : IUpdateUserPassword
    {
        private readonly IUserManager _userManager;

        public UpdateUserPassword(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResult<bool>> UpdateUserPassowrdAsync(UpdateUserPasswordModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email!);
            
            if(user == null)
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.NotFound);
            }
            //Check if the user enter the good old password
            var isOldPasswordCorrect = MyPasswordHasher.VerifyHashedPassword(user, user.Password!, model.OldPassword!);

            if (!isOldPasswordCorrect)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }

            // Hash the Password
            user.Salt = PasswordSalt.GenerateSalt();
            model.NewPassword = MyPasswordHasher.HashPassword(user, model.NewPassword!);
            user.PasswordHash = model.NewPassword;
            user.Password = model.NewPassword;
            user.IsNewPasswordRequired = false;
            user.LastModified = DateTime.UtcNow;
            user.LastModifiedBy = model.Email;
            var resultPassword = await _userManager
                .UpdatePasswordWithoutToken(user, model.NewPassword!);
            if (!resultPassword.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            }

            return new ServiceResult<bool>(true);
        }

        public async Task<ServiceResult<bool>> UpdateUserPassowrdByManagerAsync(ClaimsPrincipal claim, UpdateUserPasswordByManagerModel model)
        {
            var managerEmail = claim.Claims
                          .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
                          .Select(c => c.Value)
                          .FirstOrDefault();
            var user = await _userManager.FindByEmailAsync(model.Email!);

            if (user == null)
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.NotFound);
            }

            // Hash the Password
            user.Salt = PasswordSalt.GenerateSalt();
            model.NewPassword = MyPasswordHasher.HashPassword(user, model.NewPassword!);
            user.PasswordHash = model.NewPassword;
            user.Password = model.NewPassword;
            user.IsNewPasswordRequired = true;
            user.LastModified = DateTime.UtcNow;
            user.LastModifiedBy = managerEmail;
            var resultPassword = await _userManager
                .UpdatePasswordWithoutToken(user, model.NewPassword!);
            if (!resultPassword.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            }

            return new ServiceResult<bool>(true);
        }
    }
}
