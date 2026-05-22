
using Core.Application.Interface.Repository.SEIH;
using Core.Domain.Entity.SEIH;
using Infrastructure.Services.SEIH.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository.SEIH.Hospital;

public class InstitutionKeyRepository : IInstitutionKeyRepository
{
  
  private readonly AppDbContext _context;


    public InstitutionKeyRepository(AppDbContext context)
    {
        _context = context;
    }

    // ===============================
    // ➕ ADD
    // ===============================
    public async Task AddAsync(InstitutionKeyEntity entity)
    {
        await _context.InstitutionKeys.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // ===============================
    // 🔄 UPDATE
    // ===============================
    public async Task UpdateAsync(InstitutionKeyEntity entity)
    {
        _context.InstitutionKeys.Update(entity);
        await _context.SaveChangesAsync();
    }

    // ===============================
    // 📋 GET ALL BY HOSPITAL
    // ===============================
    public async Task<IEnumerable<InstitutionKeyEntity>> GetByHospitalAsync(Guid hospitalId)
{
    return await _context.InstitutionKeys
        .Where(k => k.HospitalId == hospitalId && k.IsDeleted != true)
        .OrderByDescending(k => k.CreatedAt)
        .ToListAsync();
}

    // ===============================
    // 🔍 GET BY VERSION
    // ===============================
    public async Task<InstitutionKeyEntity?> GetByHospitalAndVersionAsync(Guid hospitalId, int keyVersion)
{
    return await _context.InstitutionKeys
        .FirstOrDefaultAsync(k =>
            k.HospitalId == hospitalId &&
            k.KeyVersion == keyVersion &&
            k.IsDeleted != true);
}

}
