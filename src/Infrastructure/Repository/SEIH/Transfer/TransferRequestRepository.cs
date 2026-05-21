using Core.Application.Interface.Repository.SEIH;
using Core.Domain.Entity.SEIH;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.SEIH.Transfer;

public class TransferRequestRepository : ITransferRequestRepository
{
    private readonly AppDbContext _context;

    public TransferRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TransferRequestEntity entity)
    {
        await _context.Set<TransferRequestEntity>().AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<TransferRequestEntity>> GetHospitalRequestsAsync(Guid hospitalId)
    {
        return await _context.Set<TransferRequestEntity>()
            .Where(x =>
    (x.IdHospitalFrom == hospitalId || x.IdHospitalTo == hospitalId)
    && (x.IsDeleted != true))

            .ToListAsync();
    }

    public async Task<TransferRequestEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TransferRequestEntity>()
            .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != true);
    }

    public async Task<bool> UpdateAsync(TransferRequestEntity entity)
    {
        _context.Set<TransferRequestEntity>().Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
