using Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories.User;

public interface ISignInManager
{
    Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOutAsync();
    Task SignInAsync(UserEntity user, bool isPersisent);
    AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
    Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info);
    Task<ExternalLoginInfo> GetExternaLLoginInfoAsync();
} 
