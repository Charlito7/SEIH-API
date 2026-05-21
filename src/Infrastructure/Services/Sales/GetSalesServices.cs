using Application.Interfaces.Repositories.User;
using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.Sales;
using Core.Application.Interface.Services.Sales;
using Core.Application.Model.Response.Product;
using Core.Application.Model.Response.Sales;
using Core.Domain.Enums;
using Core.Domain.Procedures;
using Sprache;
using System.Net;
using System.Security.Claims;
namespace Infrastructure.Services.Sales;

public class GetSalesServices : IGetSalesService
{

    private readonly ISalesRepository _salesRepository;
    private readonly IUserManager _userManager;
    private IMapper _mapper;

    public GetSalesServices(
            IMapper mapper,
            ISalesRepository salesRepository,
            IUserManager userManager)
    {
        _mapper = mapper;
        _salesRepository = salesRepository;
        _userManager = userManager;
    }
    public async Task<ServiceResult<GetSalesPaginationResponse>> GetAllSalesMetadataServiceAsync(ClaimsPrincipal claim, int pageNumber, int pageSize)
    {

    var roles = claim.Claims
   .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
   .Select(c => c.Value)
   .ToList();
        var email = claim.Claims
         .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
         .Select(c => c.Value)
         .FirstOrDefault();

        var user = await _userManager.FindByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<GetSalesPaginationResponse>(HttpStatusCode.BadRequest);
        }

        IEnumerable<GetAllSalesMetadata> result = new List<GetAllSalesMetadata>();
        ;
        if (roles.Contains(UserRoleEnums.Admin.ToString()))
        {
            result = await _salesRepository.GetAllSalesMetadataPaginationAsync(pageNumber, pageSize);
        }
        else
        {
            result = await _salesRepository.GetAllSalesMetadataPaginationBySellerAsync(pageNumber, pageSize, user.Id);

        }



        var sales = await _salesRepository.GetAllSalesMetadataAsync();
        sales = sales.OrderByDescending(c => c.TransactionDate);
        var salesTotal = sales.Count();
        var totalPages = (int)Math.Ceiling(salesTotal / (double)pageSize);

     
        var response = new GetSalesPaginationResponse
        {
            Sales = result,
            Pagination = new PaginationInfo
            {
                Page = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalProducts = salesTotal
            }
        };

        return new ServiceResult<GetSalesPaginationResponse>(response, true, HttpStatusCode.OK, "");

    }

    public async Task<ServiceResult<IEnumerable<SalesMetadataAndProductResponseDTO>>> GetSaleDetailsServiceAsync(ClaimsPrincipal claim, Guid saleMetadataId)
    {
        var email = claim.Claims
  .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
  .Select(c => c.Value)
  .FirstOrDefault();
        var roles = claim.Claims
.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
.Select(c => c.Value)
.ToList();

        var user = await _userManager.FindByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<IEnumerable<SalesMetadataAndProductResponseDTO>>(HttpStatusCode.BadRequest);
        }
        try
        {
            IEnumerable<SalesMetadataAndProductResponse> saleDetails;
            if (roles.Contains(UserRoleEnums.Admin.ToString()))
            {
                saleDetails = await _salesRepository.GetAllSaleDetailsAsync(saleMetadataId);
                var salesMetadataResponseDTO = _mapper.Map<SalesMetadataAndProductResponseDTO[]>(saleDetails);
                return new ServiceResult<IEnumerable<SalesMetadataAndProductResponseDTO>>(salesMetadataResponseDTO, true, HttpStatusCode.OK, "");
            }
            else
            {
                saleDetails = await _salesRepository.GetSaleDetailsBySellerAsync(saleMetadataId, user.Id);
                var salesMetadataResponseDTO = _mapper.Map<SalesMetadataAndProductResponseDTO[]>(saleDetails);
                return new ServiceResult<IEnumerable<SalesMetadataAndProductResponseDTO>>(salesMetadataResponseDTO, true, HttpStatusCode.OK, "");

            }
           
        }
        catch 
        {
            return new ServiceResult<IEnumerable<SalesMetadataAndProductResponseDTO>>(null, false, HttpStatusCode.InternalServerError, "");

        }

    }
}
