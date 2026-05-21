using Core.Application.Interface.Services.Sales;
using Core.Application.Interface.Token;
using Core.Application.Model.Request.Sales;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebAPI.Controllers.Sales;
[ApiExplorerSettings(IgnoreApi = true)]
[Route("sales")]
[ApiController]
public class SalesController : BaseController
{
        private readonly ICreateSalesService _service;
    private readonly IGetSalesService _getSalesService;
    private readonly IGetSellerDailySalesResumeService _getSalesDailyResumeService;

    public SalesController(
        ICreateSalesService service, 
        IGetSalesService getSalesService,
        ITokenServices tokenServices,
        IGetSellerDailySalesResumeService getSalesDailyResumeService)
    {
        _service = service;
        _getSalesService = getSalesService;
        _getSalesDailyResumeService = getSalesDailyResumeService;
    }

    [AuthorizeRoles("Seller", "Admin")]
    [HttpPost]
    [Route("CreateSales", Name = "CreateSales")]
    public async Task<ActionResult<HttpStatusCode>> CreateSales(CreateSalesRequest sales)
    {
        if (!ModelState.IsValid)
        {
            // Extract error messages from ModelState
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Return BadRequest with error messages
            return BadRequest(new { Errors = errorMessages });
        }

        var result = await _service.CreateSalesAsync(User, sales);
        if (!result.IsOk) 
        {
            return BadRequest(result);
        }
        return Ok(result);
       
    }

    [AuthorizeRoles("Seller", "Admin", "Manager")]
    [HttpPost]
    [Route("GetSalesListPagination", Name = "GetSalesListPagination")]
    public async Task<ActionResult<HttpStatusCode>> GetSalesListAsync(int page = 1, int pageSize = 10)
    {
        var sales = await _getSalesService.GetAllSalesMetadataServiceAsync(User, page, pageSize);
        return Ok(sales);

    }
    [AuthorizeRoles("Seller", "Admin", "Manager")]
    [HttpPost]
    [Route("GetSaleDetails", Name = "GetSaleDetails")]
    public async Task<ActionResult<HttpStatusCode>> GetSaleDetailsAsync(Guid saleMetadataId)
    {
        try
        {
            var sales = await _getSalesService.GetSaleDetailsServiceAsync(User, saleMetadataId);
            return Ok(sales);
        }
        catch
        {
            return BadRequest();
        }
      

    }
    [AuthorizeRoles("Seller", "Admin", "Manager")]
    [HttpPost]
    [Route("GetDailyResume", Name = "GetDailyResume")]
    public async Task<ActionResult<HttpStatusCode>> GetDailyResumeAsync()
    {
        try
        {
            var saleResume = await _getSalesDailyResumeService.GetSellerDailySalesResumeServiceAsync(User);
            return Ok(saleResume);
        }
        catch
        {
            return BadRequest();
        }


    }
}
