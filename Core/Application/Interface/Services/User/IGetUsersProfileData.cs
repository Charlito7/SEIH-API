using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services.User;

public interface IGetUsersProfileData
{
    Task<ServiceResult<GetProfileDataResponse>> GetUserProfileDataAsync(string email);
}
