using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services.User;

public interface IMyExternalLoginService
{
    Task<ServiceResult<ExternalLoginResponse>> ExternalLoginAsync(ExternalLoginInfo info);
}
