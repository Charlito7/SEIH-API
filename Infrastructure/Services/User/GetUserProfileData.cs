using Application.Interfaces.Repositories.User;
using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Response;
using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.User
{
    public class GetUserProfileData : IGetUsersProfileData
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GetUserProfileData(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }


        public async Task<ServiceResult<GetProfileDataResponse>> GetUserProfileDataAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ServiceResult<GetProfileDataResponse>(System.Net.HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<GetProfileDataResponse>(user);

            var roles = await _userManager.GetRolesAsync(user);

            response.UserRole = GetHighestUserRole(roles);

            return new ServiceResult<GetProfileDataResponse>(response);

        }

        private string GetHighestUserRole(IList<string> roles)
        {
            var highestRole = string.Empty;

            if (roles.Contains(UserRoleEnums.Admin.ToString()))
            {
                highestRole = UserRoleEnums.Admin.ToString();
            }

            else if (roles.Contains(UserRoleEnums.Manager.ToString()))
            {
                highestRole = UserRoleEnums.Manager.ToString();    
            } 

            else
            {
                highestRole = UserRoleEnums.User.ToString();    
            }


            return highestRole;
        }
    }
}
