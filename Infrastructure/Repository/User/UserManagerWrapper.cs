using Application.Interfaces.Repositories.User;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.User
{
    public class UserManagerWrapper : IUserManager
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserManagerWrapper(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(UserEntity entity, string role)
        {
            try
            {
                return await _userManager.AddToRoleAsync(entity, role);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging and analysis
                Console.WriteLine($"Failed to add user {entity.UserName} to role {role}: {ex.Message}");

                // Rethrow the exception if you want to handle it at a higher level
                throw;

                // Or, provide a custom error message to the caller
                return IdentityResult.Failed(new IdentityError { Code = "AddToRoleFailed", Description = "Failed to add user to role. Please contact support." });
            }
        }

        public async Task<IdentityResult> CreateAsync(UserEntity userEntity, string password)
        {
            return await _userManager.CreateAsync(userEntity, password);
        }

        public async Task<UserEntity> FindByEmailAsync(string emailId)
        {
            return await _userManager.FindByEmailAsync(emailId!);
        }

        public async Task<UserEntity> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId!);
        }

        public async Task<UserEntity> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> GetPasswordResetTokenAsync(UserEntity userEntity)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(userEntity);
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserEntity userEntity, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(userEntity, token, newPassword);
        }

        public async Task<IdentityResult> UpdateAsync(UserEntity userEntity)
        {
            return await _userManager.UpdateAsync(userEntity);
        }

        public async Task<IList<string>> GetRolesAsync(UserEntity user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> DeleteAsync(UserEntity user)
        {
            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity userEntity)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            return token;
        }

        public async Task<bool> ConfirmEmail(UserEntity userEntity, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(userEntity, token);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsEmailConfirmed(UserEntity userEntity)
        {
            return await _userManager.IsEmailConfirmedAsync(userEntity);
        }

        public Task<IdentityResult> AddLoginAsync(UserEntity user, UserLoginInfo loginInfo)
        {
            return _userManager.AddLoginAsync(user, loginInfo);
        }

        public Task<IdentityResult> CreateAsync(UserEntity userEntity)
        {
            return _userManager.CreateAsync(userEntity);
        }

        public Task<IdentityResult> RemoveFromRolesAsync(UserEntity user, string roleToRemove)
        {
            return _userManager.RemoveFromRoleAsync(user, roleToRemove);
        }

        public async Task<bool> IsInRoleASync(UserEntity user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IdentityResult> CreateWithPasswordAsync(UserEntity userEntity, string password)
        {
            return await _userManager.CreateAsync(userEntity, password);
        }

        public async Task<IdentityResult> UpdatePasswordWithoutToken(UserEntity user, string newPassword)
        {
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
            return await _userManager.UpdateAsync(user);
        }
    }
}
