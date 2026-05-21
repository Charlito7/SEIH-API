using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface;
using Core.Application.Interface.Repository;
using Core.Application.Model.Request.Product;
using Core.Application.Model.Response;
using Core.Application.Model.Response.Product;
using Core.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace Infrastructure.Services.Products;

public class ProductService : IProductService
{

    private readonly IRepository<ProductEntity> _repository;
    private readonly IRepository<CategoryEntity> _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryService _categoryService;
    private IMapper _mapper;

    public ProductService(
            IRepository<ProductEntity> repository, 
            ICategoryService categoryService,
            IMapper mapper, 
            IRepository<CategoryEntity> categoryRepository,
            IProductRepository productRepository)
    {

        _repository = repository;
        _categoryRepository = categoryRepository;
        _categoryService = categoryService;
        _mapper = mapper;
        _productRepository = productRepository;
    }



    public async Task<ServiceResult<IEnumerable<ProductRequest>>> CreateProductsAsync(List<ProductRequest> requests)
    {
        var failedRequests = new List<ProductRequest>();

        foreach (var request in requests)
        {
            var categoryResult = await _categoryService.GetCategoryByNameAsync(request.CategoryName);

            // Ensure that the category was found
            if (categoryResult?.Result == null)
            {
                failedRequests.Add(request);
                continue;
            }

            var productEntity = new ProductEntity()
            {
                Name = request.Name,
                Description = request.Description,
                BarCode = request.BarCode,
                CategoryId = categoryResult.Result.CategoryId,
                ExpiredDate = request.ExpiredDate,
                Quantity = request.Quantity,
                Price = request.Price,
                MinimumReorderQuantity = request.MinimumReorderQuantity,
                IsReturnAccepted = request.IsReturnAccepted,
                ReturnTimeAccepted = request.ReturnTimeAccepted
            };

            var isCreated = await _repository.CreateAsync(productEntity);

            if (!isCreated)
            {
                failedRequests.Add(request);
            }
        }

        // If no failed requests, return success
        if (failedRequests.Count == 0)
        {
            return new ServiceResult<IEnumerable<ProductRequest>>(requests, true, HttpStatusCode.OK, "All products added successfully.");
        }
        else
        {
            return new ServiceResult<IEnumerable<ProductRequest>>(failedRequests, false, HttpStatusCode.BadRequest, "Some products were not saved successfully.");
        }
    }



    public Task<ServiceResult<HttpStatusCode>> DeleteProductAsync(Guid productId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<ProductResponse>> GetProductByBarCodeAsync(string bareCode)
    {
        if (string.IsNullOrEmpty(bareCode))
        {
            return new ServiceResult<ProductResponse>(HttpStatusCode.BadRequest);
        }

        var result = await _repository.FindAsync(product => product.BarCode.Contains(bareCode) && product.Quantity >= 0);

        if (result == null)
        {
            return new ServiceResult<ProductResponse>(HttpStatusCode.NotFound);
        }

        var response = _mapper.Map<ProductResponse>(result);

        var category = await _categoryRepository.FindAsync(p => p.Id == result.CategoryId);

        if (category == null)
        {
            response.CategoryName = "N/A";
        }

        response.CategoryName = category.Name;
        return new ServiceResult<ProductResponse>(response);

    }

    public async Task<ServiceResult<ProductResponse>> GetProductByIdAsync(string productId)
    {
        if (string.IsNullOrEmpty(productId))
        {
            return new ServiceResult<ProductResponse>(HttpStatusCode.BadRequest);
        }

        var result = await _repository.GetByIdAsync(Guid.Parse(productId));
        if (result == null)
        {
            return new ServiceResult<ProductResponse>(HttpStatusCode.NotFound);
        }

        var response = _mapper.Map<ProductResponse>(result);
        var category = await _categoryRepository.FindAsync(p => p.Id == result.CategoryId);

        if (category == null)
        {
            response.CategoryName = "N/A";
        }

        response.CategoryName = category.Name;
        return new ServiceResult<ProductResponse>(response);
    }

    public async Task<ServiceResult<ProductRequest>> GetProductByNameAsync(string name)
    {
        var result = await _repository.FindListAsync(product => product.Name.Contains(name));

        if (result.Count > 0)
        {
            // Retrieve the first category from the result
            var firstCategory = result[0];

            // convert the entity in category model
            ProductRequest productModel = new ProductRequest()
            {
                Description = firstCategory.Description,
                Name = firstCategory.Name,
            };

            // Now, you can use categoryId as needed
            // For example, you might want to return it as part of your ServiceResult
            return new ServiceResult<ProductRequest>(productModel, true, HttpStatusCode.OK, "Success");
        }
        else
        {
            // Handle the case where no category was found with the specified name
            // You might want to return an error message or handle it in a way that fits your application's logic
            return new ServiceResult<ProductRequest>(new ProductRequest(), false, HttpStatusCode.BadRequest, "Failure");
        }
    }

    public async Task<ServiceResult<IEnumerable<ProductRequest>>> GetProductsAsync()
    {
        var result = await _repository.GetAllAsync(p => p.Category);
        IEnumerable<ProductRequest> product = result
        .Select(productEntity => new ProductRequest
        {
            Id = (Guid)productEntity.Id,
            Name = productEntity.Name,
            Description = productEntity.Description,
            BarCode = productEntity.BarCode,
            ExpiredDate = productEntity.ExpiredDate,
            CategoryName = productEntity.Category.Name,
            Price = productEntity.Price,
            Quantity = productEntity.Quantity,
            MinimumReorderQuantity = productEntity.MinimumReorderQuantity,
            IsReturnAccepted = productEntity.IsReturnAccepted,
            ReturnTimeAccepted = productEntity.ReturnTimeAccepted,
            // Add other properties as needed
        });

        return new ServiceResult<IEnumerable<ProductRequest>>(product, true, HttpStatusCode.OK, "Category is added with success");
    }

    public async Task<ServiceResult<GetProductPaginationResponse>> GetProductsWithPaginationAsync(int page = 1, int pageSize = 10)
    {
        // Calculate the number of items to skip based on the current page and page size
        var skip = (page - 1) * pageSize;

        // Retrieve paginated products and include the related category
        var result = await _repository.GetAllPaginationAsync(skip, pageSize, p => p.Category);
         
        // Convert the result to the desired ProductRequest model
        IEnumerable<ProductRequest> product = result
            .Select(productEntity => new ProductRequest
            {
                Id = (Guid)productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                BarCode = productEntity.BarCode,
                ExpiredDate = productEntity.ExpiredDate,
                CategoryName = productEntity.Category.Name,
                Price = productEntity.Price,
                Quantity = productEntity.Quantity,
                MinimumReorderQuantity = productEntity.MinimumReorderQuantity,
                IsReturnAccepted = productEntity.IsReturnAccepted,
                ReturnTimeAccepted = productEntity.ReturnTimeAccepted,
            });

        // Optionally: Get the total count of products for pagination metadata
        var totalProducts = (await _repository.GetAllAsync()).Count;
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize); 

        // Build the response with pagination metadata
        var response = new GetProductPaginationResponse
        {
            Products = product,
            Pagination = new PaginationInfo
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalProducts = totalProducts
            }
        };

        return new ServiceResult<GetProductPaginationResponse>(response, true, HttpStatusCode.OK, "Category is added with success");
    }

    public async Task<IEnumerable<GetProductSuggestionsResponse>> GetProductSuggestions(string userInput)
    {
        var dbResult = await _productRepository.GetProductSuggestions(userInput);
        return _mapper.Map<List<GetProductSuggestionsResponse>>(dbResult);
    }
}
