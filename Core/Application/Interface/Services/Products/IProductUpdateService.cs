using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Request.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interface;

public interface IProductUpdateService
{
    public Task<ServiceResult<ProductRequest>> UpdateProductsAsync(ProductRequest requests);
    public Task<ServiceResult<IEnumerable<ProductRequest>>> UpdateProductsAsync(List<ProductRequest> requests);
}
