using Application.Interfaces.Repositories.User;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.User
{
    public class SignInManagerWrapper : ISignInManager
    {
        private readonly SignInManager<UserEntity> _signInManager;

        public SignInManagerWrapper(SignInManager<UserEntity> signInManager)
        {
            _signInManager = signInManager;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info)
        {
            return await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        }

        public async Task<ExternalLoginInfo> GetExternaLLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging and troubleshooting
                //_logger.LogError(ex, "Password sign-in failed");
                Console.WriteLine(ex);

                // Customize error handling and messaging based on the exception type
                throw; // Rethrow to allow higher-level exception handling
            }
        }

        public async Task SignInAsync(UserEntity user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
