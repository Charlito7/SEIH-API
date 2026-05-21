using Application.Interfaces.Repositories.User;
using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.Sales;
using Core.Application.Interface.Services.Sales;
using Core.Application.Model.Response.Sales;
using Core.Domain.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Sales;

public class GetSellerDailySalesResumeService : IGetSellerDailySalesResumeService
{
    private readonly ISalesRepository _salesRepository;
    private readonly IUserManager _userManager;
    private IMapper _mapper;

    public GetSellerDailySalesResumeService(
            IMapper mapper,
            ISalesRepository salesRepository,
            IUserManager userManager)
    {
        _mapper = mapper;
        _salesRepository = salesRepository;
        _userManager = userManager;
    }
    public async Task<ServiceResult<GetSellerDailySalesResumeResponse>> GetSellerDailySalesResumeServiceAsync(ClaimsPrincipal claim)
    {
        var email = claim.Claims
         .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
         .Select(c => c.Value)
         .FirstOrDefault();

        var user = await _userManager.FindByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<GetSellerDailySalesResumeResponse>(HttpStatusCode.BadRequest);
        }
        GetSellerDailySalesResumeResponse response = new GetSellerDailySalesResumeResponse();
        var dailyResponse = await _salesRepository.GetSellerDailyResumeAsync(Guid.Parse(user.Id));
        if (dailyResponse == null) {
            response.AmountIN = 0;
        }
        var totalDaily = await _salesRepository.GetSellerSalesTotalPriceAndQuantityTodayAsync(user.Id);
      
        response.AmountIN = dailyResponse.AmountIN;

        if (totalDaily.Count() == 0) {
            response.TotalQuantitySales = 0;
            response.TotalPriceSales = 0;
        }
        foreach (var total in totalDaily) {
            response.TotalPriceSales = total.TotalPriceSales;
            response.TotalQuantitySales = total.TotalQuantitySales;
            response.Total = response.TotalPriceSales + response.AmountIN;
        }
        return new ServiceResult<GetSellerDailySalesResumeResponse>(response, true, HttpStatusCode.OK, "");
    }
}
