using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Request.Sales;
using Core.Application.Model.Response.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interface.Services.Sales;

public interface ICreateSalesService
{
    public Task<ServiceResult<CreateSalesResponse>> CreateSalesAsync(ClaimsPrincipal user,CreateSalesRequest requests);
    public Task<ServiceResult<List<GetSalesListResponse>>> GetSalesListAsync(ClaimsPrincipal user);

}
