using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface;
using Core.Application.Interface.Services.Sales;
using Core.Application.Model.Request.Sales;
using Core.Application.Model.Response.Sales;
using Core.Domain.Entity;
using Core.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Infrastructure.Services.Sales
{
    public class CreateSalesMetadataService : ICreateSalesMetadataService
    {
        private readonly IRepository<SalesMetadataEntity> _repository;
        private IMapper _mapper;

        public CreateSalesMetadataService(IRepository<SalesMetadataEntity> repository,
            IMapper mapper)
        {

            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult<CreateSalesMetadataResponse>> CreateSalesMetadataAsync(CreateSalesMetadataRequest request)
        {

            //Check seller info

            //check 
            if(request.PaymentType == PaymentMethodEnum.MONCASH.ToString() || request.PaymentType == PaymentMethodEnum.NATCASH.ToString() || request.PaymentType == PaymentMethodEnum.PAV.ToString())
            {
                if(string.IsNullOrEmpty(request.PaymentTypeTransactionID))
                {
                    return new ServiceResult<CreateSalesMetadataResponse>(new CreateSalesMetadataResponse(), false, HttpStatusCode.BadRequest,
               "Error on the refund");
                }
            }

            var salesMetadataEntity = new SalesMetadataEntity()
            {
                TransactionDate =DateTime.Now,
                CustomerCode = "N/A",
                OrderTaxPercentage = 0,
                ShippingCost = 0,
                ShippingAddress = "N/A",
                Status = SalesStatusEnum.DELIVERED.ToString(),
                Notes = request.Notes ?? string.Empty,
                SellerCode = "TBD",
                TotalAmount = 0,
                CashReceived = request.CashReceived,
                PaymentType = request.PaymentType.ToString(),
                PaymentTypeTransactionID = request.PaymentTypeTransactionID

            };
            var result = await _repository.CreateAsync(salesMetadataEntity);
            throw new NotImplementedException();
        }
    }
}
