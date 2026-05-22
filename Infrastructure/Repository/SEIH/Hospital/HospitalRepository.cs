
using Core.Application.Interface.Repository.SEIH;
using Core.Domain.Entity.SEIH;
using Infrastructure.Services.SEIH.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository.SEIH.Hospital;

public class HospitalRepository : IHospitalRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public HospitalRepository(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task AddAsync(HospitalEntity hospital)
    {
        await _context.Hospitals.AddAsync(hospital);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
        if (hospital == null)
            throw new Exception("Hospital not found");

        hospital.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<HospitalEntity>> GetAllAsync()
    {
        return await _context.Hospitals
            .Where(h => h.IsDeleted == false || h.IsDeleted == null)
            .ToListAsync();
    }

    public async Task<HospitalEntity?> GetHospitalByIdAsync(Guid hospitalId)
    {
        if (string.IsNullOrWhiteSpace(hospitalId.ToString()))
            return null;

        return await _context.Hospitals
                             .Where(u => u.Id == hospitalId && (u.IsDeleted == null || u.IsDeleted == false))
                             .FirstOrDefaultAsync();
    }

    public async Task<HospitalEntity?> GetHospitalByNameAsync(string hospitalName)
    {
        if (string.IsNullOrWhiteSpace(hospitalName))
            return null;

        return await _context.Hospitals
                             .Where(u => u.Name == hospitalName && (u.IsDeleted == null || u.IsDeleted == false))
                             .FirstOrDefaultAsync();
    }

    public async Task<HospitalEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Hospitals
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task UpdateAsync(HospitalEntity hospital)
    {
        _context.Hospitals.Update(hospital);
        await _context.SaveChangesAsync();
    }

    public async Task<HospitalEntity?> GetByThumbprintAsync(string thumbprint)
    {
        return await _context.Hospitals
            .FirstOrDefaultAsync(h =>
                h.CertificateThumbprint == thumbprint &&
                (h.IsDeleted == null || h.IsDeleted == false));
    }

    public async Task<HospitalEntity?> GetByApiKeyAsync(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return null;

        return await _context.Hospitals
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.ApiKey == apiKey);
    }

    public async Task<HospitalEntity?> GetByCertificateThumbprintAsync(string thumbprint)
    {
        if (string.IsNullOrWhiteSpace(thumbprint))
            return null;

        return await _context.Hospitals
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.CertificateThumbprint == thumbprint);
    }

}
