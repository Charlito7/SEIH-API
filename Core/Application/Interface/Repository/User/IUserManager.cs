using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories.User;

public interface IUserManager
{
    Task<UserEntity> FindByIdAsync(string userId);
    Task<UserEntity> FindByNameAsync(string userName);
    Task<UserEntity> FindByEmailAsync(string emailId);
    Task<IdentityResult> CreateAsync(UserEntity userEntity, string password);
    Task<IdentityResult> CreateAsync(UserEntity userEntity);
    Task<IdentityResult> CreateWithPasswordAsync(UserEntity userEntity, string password);
    Task<IdentityResult> UpdateAsync(UserEntity userEntity);
    Task<bool> DeleteAsync(UserEntity user);
    Task<string> GetPasswordResetTokenAsync(UserEntity userEntity);
    Task<IdentityResult> ResetPasswordAsync(UserEntity userEntity, string token, string newPassword);
    Task<string> GenerateEmailConfirmationTokenAsync(UserEntity userEntity);
    Task<bool> ConfirmEmail(UserEntity userEntity, string token);
    Task<bool> IsEmailConfirmed(UserEntity userEntity);
    Task<IdentityResult> AddToRoleAsync(UserEntity entity, string role);
    Task<IList<string>> GetRolesAsync(UserEntity user);
    Task<IdentityResult> AddLoginAsync(UserEntity user, UserLoginInfo loginInfo);
    Task<bool> IsInRoleASync(UserEntity user, string role);
    Task<IdentityResult> RemoveFromRolesAsync(UserEntity user, string roleToRemove);
    Task<IdentityResult> UpdatePasswordWithoutToken(UserEntity user, string newPassword);
}
