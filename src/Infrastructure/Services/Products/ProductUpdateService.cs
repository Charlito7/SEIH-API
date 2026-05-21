using Core.Application.Commons.ServiceResult;
using Core.Application.Interface;
using Core.Application.Model.Request.Product;
using Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class ProductUpdateService : IProductUpdateService
{
    private readonly IRepository<ProductEntity> _repository;
    private readonly ICategoryService _categoryService;

    public ProductUpdateService(IRepository<ProductEntity> repository, ICategoryService categoryService)
    {

        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _categoryService = categoryService;
    }
    public async Task<ServiceResult<ProductRequest>> UpdateProductsAsync(ProductRequest request)
    {
        // Check if the ID is null
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        if (request.Id == Guid.Empty || request.Id == null)
        {
            return new ServiceResult<ProductRequest>(request, false, HttpStatusCode.BadRequest, "BadRequest code 1");
        }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
           
        // Check if the product exist
        var checkProduct = await _repository.GetByIdAsync(request.Id);
        if (checkProduct == null)
        {
            return new ServiceResult<ProductRequest>(request, false, HttpStatusCode.BadRequest, "BadRequest code 2");
        }
        // Check if the Category exist
        var categoryCheck = await _categoryService.GetCategoryByNameAsync(request.CategoryName);
        if (categoryCheck == null)
        {
            return new ServiceResult<ProductRequest>(request, false, HttpStatusCode.BadRequest, "BadRequest code 3");
        }

        checkProduct.Name = request.Name;
        checkProduct.Description = request.Description;
        checkProduct.BarCode = request.BarCode;
        checkProduct.CategoryId = categoryCheck.Result.CategoryId;
        checkProduct.ExpiredDate = request.ExpiredDate;
        checkProduct.Quantity = request.Quantity;
        checkProduct.Price = request.Price;
        checkProduct.MinimumReorderQuantity = request.MinimumReorderQuantity;

        var result = await _repository.UpdateAsync(checkProduct);

        if (!result)
        {
            return new ServiceResult<ProductRequest>(request, false, HttpStatusCode.InternalServerError, "InternalServerError Code 1");
        }

        return new ServiceResult<ProductRequest>(request, true, HttpStatusCode.OK, "All products added with success.");
  
    }


    public async Task<ServiceResult<IEnumerable<ProductRequest>>> UpdateProductsAsync(List<ProductRequest> requests)
    {
        var results = new List<ProductRequest>();

        foreach (var request in requests)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product == null)
            {
                results.Add(request);
                continue;
            }
            var categoryResult = await _categoryService.GetCategoryByNameAsync(request.CategoryName);

            if (categoryResult == null)
            {
                results.Add(request);
                continue;
            }

            // Apply the change
            product.Name = request.Name;
            product.Description = request.Description;
            product.BarCode = request.BarCode;
            product.CategoryId = categoryResult.Result.CategoryId;
            product.Price = request.Price;
            product.ExpiredDate = request.ExpiredDate;
            product.Quantity =
            product.MinimumReorderQuantity = request.MinimumReorderQuantity;

            var result = await _repository.UpdateAsync(product);

            if (!result)
            {
                results.Add(request);
            }
        }

        // Check if all products were added successfully
        if (results.Count > 0)
        {
            return new ServiceResult<IEnumerable<ProductRequest>>(results, false, HttpStatusCode.BadRequest, "Some products were not saved successfully.");
        }
        else
        {
            return new ServiceResult<IEnumerable<ProductRequest>>(results, true, HttpStatusCode.OK, "All products added with success.");
           
        }
    }
    private ProductEntity TempMapper(ProductEntity product, ProductRequest model)
    {
        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            product.Name = model.Name;
        }
        if (!string.IsNullOrWhiteSpace(model.Description))
        {
            product.Description = model.Description;
        }
        if (!string.IsNullOrWhiteSpace(model.BarCode))
        {
            product.BarCode = model.BarCode;
        }

        return product;
    }

}
