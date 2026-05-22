using Core.Application.Interface.Repository.SEIH.Hospital;
using Infrastructure.Services.SEIH.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repository.SEIH.Hospital;

public class HospitalPermissionRepository : IHospitalPermissionRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public HospitalPermissionRepository(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetPermissionAsyncRepository()
    {
        return await _context.Permissions
            .Where(p => !p.IsCreatedBySEIH)
            .Select(p => p.Name!)
            .ToListAsync();
    }


}
