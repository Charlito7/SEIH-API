using Core.Application.Interface.Repository;
using Core.Application.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Product;

public class ProductRepository: IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GetProductSuggestionsResponse>> GetProductSuggestions(string userInput)
    {
        return await _context.ProductSuggestionsResponses
        .FromSqlRaw("CALL GetAllProductSuggestions({0})", userInput)
        .ToListAsync();
    }
}
