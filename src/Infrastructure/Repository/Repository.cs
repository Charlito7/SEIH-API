using Core.Application.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _entities;

    public Repository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _entities = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        var query = _entities.AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<List<T>> GetAllPaginationAsync(int skip, int take, params Expression<Func<T, object>>[] includes)
    {
        var query = _entities.AsQueryable();

        // Apply includes if any
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply pagination
        query = query.Skip(skip).Take(take);

        return await query.ToListAsync();
    }


    public async Task<T> GetByIdAsync(Guid id)
    {

        return await _entities.FindAsync(id);

    }

    public async Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.Where(predicate).ToListAsync();
    }
    public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> CreateAsync(T entity)
    {
        try
        {
            _entities.Add(entity);
            int affectedRows = await _context.SaveChangesAsync();

            // Check if any rows were affected during the save operation
            return affectedRows > 0;
        }
        catch (DbUpdateException ex)
        {
            // Log the exception or handle it accordingly
            Console.Error.WriteLine($"Error saving changes to the database: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Handle other exceptions, log them, and return false
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<T> CreateTAsync(T entity)
    {
        try
        {
            _entities.Add(entity); // Add the entity to the DbSet
            await _context.SaveChangesAsync(); // Save the changes to the database

            // Return the entity that was created
            return entity;
        }
        catch (DbUpdateException ex)
        {
            // Log the exception or handle it accordingly
            Console.Error.WriteLine($"Error saving changes to the database: {ex.Message}");
            return null; // Return null if the operation fails
        }
        catch (Exception ex)
        {
            // Handle other exceptions, log them, and return null
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
            return null;
        }
    }



    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            int affectedRows = await _context.SaveChangesAsync();

            // Check if any rows were affected during the save operation
            return affectedRows > 0;
        }
        catch (DbUpdateException ex)
        {
            // Log the exception or handle it accordingly
            Console.Error.WriteLine($"Error saving changes to the database: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Handle other exceptions, log them, and return false
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
            return false;
        }
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var entity = await _entities.FindAsync(predicate);
        if (entity != null)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}