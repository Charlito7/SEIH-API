using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Request.Product;
using Core.Application.Model.Response;
using Core.Application.Model.Response.Product;
using System.Net;

namespace Core.Application.Interface;

public interface IProductService
{
    public Task<ServiceResult<IEnumerable<ProductRequest>>> GetProductsAsync();
    public Task<ServiceResult<ProductResponse>> GetProductByIdAsync(string productId);
    public Task<ServiceResult<ProductResponse>> GetProductByBarCodeAsync(string bareCode);
    public Task<ServiceResult<IEnumerable<ProductRequest>>> CreateProductsAsync(List<ProductRequest> requests);
    public Task<ServiceResult<HttpStatusCode>> DeleteProductAsync(Guid productId);
    public Task<ServiceResult<ProductRequest>> GetProductByNameAsync(string name);
    public Task<ServiceResult<GetProductPaginationResponse>> GetProductsWithPaginationAsync(int page = 1, int pageSize = 10);
    Task<IEnumerable<GetProductSuggestionsResponse>> GetProductSuggestions(string userInput);
}
