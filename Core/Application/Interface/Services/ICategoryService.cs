using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Request;
using System.Net;

namespace Core.Application.Interface;

public interface ICategoryService
{
    public Task<ServiceResult<IEnumerable<CreateCategoryRequest>>> GetCategoryAsync();
    public Task<ServiceResult<CreateCategoryRequest>> GetCategoryByIdAsync(int productId);
    public Task<ServiceResult<GetCategoryRequest>> GetCategoryByNameAsync(string name);
    public Task<ServiceResult<HttpStatusCode>> CreateCategoryAsync(CreateCategoryRequest request);
    public Task<ServiceResult<HttpStatusCode>> UpdateCategoryAsync(int productId, CreateCategoryRequest request);
    public Task<ServiceResult<HttpStatusCode>> DeleteCategoryAsync(int productId);
}
