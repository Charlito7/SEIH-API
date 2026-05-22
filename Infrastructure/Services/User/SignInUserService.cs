using Application.Interfaces.Repositories.User;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Token;
using Core.Application.Interfaces.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.User
{
    public class SignInUserService : ISignInUserService
    {
        public readonly ISignInManager _signInManager;
        public readonly IUserManager _userManager;
        public readonly ITokenServices _tokenServices;

        public SignInUserService(ISignInManager signInManager, IUserManager userManager, 
            ITokenServices tokenServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenServices = tokenServices;
        }

        public async Task<ServiceResult<bool>> SignInAsync(string userName, string password)
        {
            return new ServiceResult<bool>(true);
        }
    }
}
