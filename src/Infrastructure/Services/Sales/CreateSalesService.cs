using Application.Interfaces.Repositories.User;
using AutoMapper;
using Azure.Core;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface;
using Core.Application.Interface.Services.Sales;
using Core.Application.Model.Request;
using Core.Application.Model.Request.Sales;
using Core.Application.Model.Response;
using Core.Application.Model.Response.Sales;
using Core.Domain.Entity;
using Core.Domain.Enums;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Sales;

public class CreateSalesService : ICreateSalesService
{
    private readonly IRepository<SalesMetadataEntity> _salesMetadataRepository;
    private readonly IRepository<ProductSalesEntity> _productSalesRepository;
    private readonly IRepository<ProductEntity> _productRepository;
    private readonly IUserManager _userManager;
    private IMapper _mapper;
    private readonly AppDbContext _applicationDbContext;

    public CreateSalesService(IRepository<SalesMetadataEntity> salesMetadataRepository,
                              IRepository<ProductSalesEntity> productSalesRepository,
                              IRepository<ProductEntity> productRepository,
                              IMapper mapper,
                              IUserManager userManager,
                              AppDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _salesMetadataRepository = salesMetadataRepository;
        _productSalesRepository = productSalesRepository;
        _productRepository = productRepository;
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<ServiceResult<CreateSalesResponse>> CreateSalesAsync(ClaimsPrincipal claim, CreateSalesRequest request)
    {
        List<ProductEntity> productsListReachedTarget = new List<ProductEntity>();

        var email = claim.Claims
        .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
        .Select(c => c.Value)
        .FirstOrDefault();

        var user = await _userManager.FindByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<CreateSalesResponse>(HttpStatusCode.BadRequest);
        }

        CreateSalesResponse salesResponse = new CreateSalesResponse();
        decimal amount = 0;

        //check 
        if (request.SalesMetadata.PaymentType.ToUpper() == PaymentMethodEnum.MONCASH.ToString() || request.SalesMetadata.PaymentType.ToUpper() == PaymentMethodEnum.NATCASH.ToString() || request.SalesMetadata.PaymentType.ToUpper() == PaymentMethodEnum.PAV.ToString())
        {
            if (string.IsNullOrEmpty(request.SalesMetadata.PaymentTypeTransactionID))
            {
                return new ServiceResult<CreateSalesResponse>(salesResponse, false, HttpStatusCode.BadRequest,
                    request.SalesMetadata.PaymentType+ " Transaction ID is missing");

            }
        }

        List<ProductEntity> productsList = new List<ProductEntity>();
        foreach (var req in request.ProductSales) {
            var prod = await _productRepository.FindAsync(c => c.BarCode == req.ProductCode);
            if (prod == null) {
                return new ServiceResult<CreateSalesResponse>(salesResponse, false, HttpStatusCode.BadRequest,
                    "Product name: " + req.ProductName + " not on sales please remove it");
            }

         

            if (prod.Price != req.UnitCost)
            {
                return new ServiceResult<CreateSalesResponse>(salesResponse, false, HttpStatusCode.BadRequest, 
                    "We have only " + prod.Quantity + "The price for " + req.ProductName + " is update, remove it and scan it again");
            }

            if (req.Quantity > prod.Quantity) {
                return new ServiceResult<CreateSalesResponse>(salesResponse, false, HttpStatusCode.BadRequest, "We have only " + prod.Quantity + " available   for remove it and scan it again, to continue");
            }
            amount += prod.Price*req.Quantity;

            //update the quantity for future using
            prod.Quantity -= req.Quantity;
            productsList.Add(prod);

            //Check if the target is
            if(prod.Quantity <= prod.MinimumReorderQuantity)
            {
                productsListReachedTarget.Add(prod);
            }
        }

        if(request.SalesMetadata.CashReceived < amount - request.SalesMetadata.Discount)
        {
            return new ServiceResult<CreateSalesResponse>(salesResponse, false, HttpStatusCode.BadRequest,
                     "Lack of liquidity");
        }
        if (_applicationDbContext.Database.CurrentTransaction != null)
        {
            Console.WriteLine(Environment.StackTrace); // Logs the call stack
            Console.WriteLine("Transaction already started! Stack trace:");
            
        }

        var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

        try
            {
                // Save the Metadata
                var salesMetadataEntity = new SalesMetadataEntity()
                {
                    TransactionDate = DateTime.UtcNow,
                    CustomerCode = "N/A",
                    OrderTaxPercentage = 0,
                    Discount = request.SalesMetadata.Discount,
                    ShippingCost = 0,
                    ShippingAddress = "N/A",
                    Status = SalesStatusEnum.DELIVERED.ToString(),
                    Notes = request.SalesMetadata.Notes ?? string.Empty,
                    SellerCode = user.Id,
                    TotalAmount = amount,
                    CashReceived = request.SalesMetadata.CashReceived,
                    PaymentType = request.SalesMetadata.PaymentType.ToString(),
                    PaymentTypeTransactionID = request.SalesMetadata.PaymentTypeTransactionID,
                    PaymentCustomerName = request.SalesMetadata.PaymentCustomerName,
                    CreatedBy = user.Id,
                    LastModifiedBy = user.Id,
                };

              await _applicationDbContext.SalesMetadata.AddAsync(salesMetadataEntity);
          //    await _applicationDbContext.SaveChangesAsync();

                List<ProductSalesEntity> productsSalesList = new List<ProductSalesEntity>();

                // Save the Products sales
                foreach (var item in request.ProductSales)
                {
                    ProductSalesEntity product = new ProductSalesEntity()
                    {
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        UnitCost = item.UnitCost,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        SalesMetadataId = salesMetadataEntity.Id,
                        CreatedBy = user.Id,
                        LastModifiedBy = user.Id,
                    };

                // Add the product to the list
                productsSalesList.Add(product);
                }

            // Add product sales to the database
           await _applicationDbContext.AddRangeAsync(productsSalesList);


            //Update Product quantity the quantity
            _applicationDbContext.UpdateRange(productsList);
               
            await _applicationDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
               
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResult<CreateSalesResponse>(
                    salesResponse, false, HttpStatusCode.InternalServerError,
                    "An error occurred: " + ex.Message
                );
            }
        

        if(productsListReachedTarget.Count > 0)
        {
            //Send an email
        }

        return new ServiceResult<CreateSalesResponse>(salesResponse, true, HttpStatusCode.OK, "");

    }

    public async Task<ServiceResult<List<GetSalesListResponse>>> GetSalesListAsync(ClaimsPrincipal claim)
    {
        var email = claim.Claims
    .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
    .Select(c => c.Value)
    .FirstOrDefault();

        var user = await _userManager.FindByEmailAsync(email!);
        
        throw new NotImplementedException();
    }
}
