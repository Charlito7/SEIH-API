using Application.Interfaces.Repositories.User;
using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;

namespace Infrastructure.Services.User
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IMapper _mapper;
        private readonly ISignInManager _signInManager;

        public CreateUserService(IUserManager userManager, IRoleManager roleManager, IMapper mapper, ISignInManager signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<ServiceResult<bool>> CreateUserAsync(CreateUserModel dataModel)
        {
            var user = _mapper.Map<UserEntity>(dataModel);

            var isUserExists = await _userManager.FindByEmailAsync(dataModel.Email!);

            if(isUserExists != null)
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.NotAcceptable);
            }
            // Hash the Password
            user.Salt = PasswordSalt.GenerateSalt();
            dataModel.Password = MyPasswordHasher.HashPassword(user, dataModel.Password);
            user.PasswordHash = dataModel.Password;
            user.Password = dataModel.Password;
            user.IsNewPasswordRequired = true;
            var result = await _userManager
                .CreateWithPasswordAsync(user, dataModel.Password!);
            if(!result.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);  
            }
            //var loginResult = await _userManager.AddLoginAsync(user, info);

            //if (!loginResult.Succeeded)
            //{
            //    return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            //}

            //await _signInManager.SignInAsync(user, false);

            await _userManager.AddToRoleAsync(user, UserRoleEnums.User.ToString());

            //var isRoleExists = await _roleManager.IsRoleExists(UserRoleEnums.User.ToString());
            //if (!isRoleExists)
            //{
            //    var userRole = await _roleManager.CreateRoleAsync(UserRoleEnums.User.ToString());
            //    await _userManager.AddToRoleAsync(user, UserRoleEnums.User.ToString());
            //}
            //else
            //{
            //    await _userManager.AddToRoleAsync(user, UserRoleEnums.User.ToString());
            //}

            //var createdUser = await _userManager.FindByEmailAsync(user.Email!);

            return new ServiceResult<bool>(true);
        }

        public async Task<ServiceResult<bool>> CreateUserAsync(CreateUserModel dataModel, ExternalLoginInfo info)
        {
            var user = _mapper.Map<UserEntity>(dataModel);

            var domain = dataModel.Email!.Split('@')[1].ToString();

            if (domain.ToLower() != "gmail.com")
            {
                return new ServiceResult<bool>(HttpStatusCode.Forbidden);
            }

            var isUserExists = await _userManager.FindByEmailAsync(dataModel.Email!);

            if (isUserExists != null)
            {
                return new ServiceResult<bool>(System.Net.HttpStatusCode.NotAcceptable);
            }

            var result = await _userManager
                .CreateAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            }
            var loginResult = await _userManager.AddLoginAsync(user, info);

            if (!loginResult.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            }

            await _signInManager.SignInAsync(user, false);

            var isRoleExists = await _roleManager.IsRoleExists(UserRoleEnums.User.ToString());
            if (!isRoleExists)
            {
                var userRole = await _roleManager.CreateRoleAsync(UserRoleEnums.User.ToString());
                await _userManager.AddToRoleAsync(user, UserRoleEnums.User.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(user, UserRoleEnums.User.ToString());
            }

            return new ServiceResult<bool>(true);
        }
    }
}
