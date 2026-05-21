using AutoMapper;
using Core.Application.Model.Request;
using Core.Application.Model.Response;
using Core.Application.Model.Response.Product;
using Core.Domain.Entities;
using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;
using Core.Domain.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapper.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<CreateUserModel, UserEntity>();
        CreateMap<UserEntity, GetProfileDataResponse>();
        CreateMap<ProductEntity, ProductResponse>();
        CreateMap<SalesMetadataAndProductResponse, SalesMetadataAndProductResponseDTO>()
           .ForMember(dest => dest.ProductSales, opt => opt.MapFrom(src =>
               string.IsNullOrEmpty(src.ProductSalesDetails)
                   ? new List<ProductSaleDetail>()
                   : JsonConvert.DeserializeObject<List<ProductSaleDetail>>(src.ProductSalesDetails)));

        CreateMap<CreateUserModel, UsersEntity>();
    }


}
