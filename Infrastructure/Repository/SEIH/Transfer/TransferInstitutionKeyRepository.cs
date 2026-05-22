using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Domain.Entity.SEIH;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.SEIH.Hospital;

public class TransferInstitutionKeyRepository : ITransferInstitutionKeyRepository
{
    private readonly AppDbContext _context;

    public TransferInstitutionKeyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TransferInstitutionKeyEntity entity)
    {
        await _context.TransferInstitutionKeys.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TransferInstitutionKeyEntity entity)
    {
        _context.TransferInstitutionKeys.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TransferInstitutionKeyEntity>> GetByHospitalAsync(Guid hospitalId)
    {
        return await _context.TransferInstitutionKeys
            .Where(x => x.HospitalId == hospitalId && x.IsDeleted != true)
            .ToListAsync();
    }

    public async Task<TransferInstitutionKeyEntity?> GetActiveByHospitalAsync(Guid hospitalId)
    {
        return await _context.TransferInstitutionKeys
            .FirstOrDefaultAsync(x =>
                x.HospitalId == hospitalId &&
                x.IsActive &&
                x.IsDeleted != true);
    }

    public async Task<TransferInstitutionKeyEntity?> GetByHospitalAndVersionAsync(Guid hospitalId, int keyVersion)
    {
        return await _context.TransferInstitutionKeys
            .FirstOrDefaultAsync(x =>
                x.HospitalId == hospitalId &&
                x.KeyVersion == keyVersion &&
                x.IsDeleted != true);
    }
}