using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Domain.Entity.SEIH;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.SEIH.Hospital;

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;

    public TransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateTransferAsync(TransferEntity entity)
    {
        await _context.Set<TransferEntity>().AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<TransferEntity>> 
        GetTransfersByHospitalAsync(Guid hospitalId)
    {
        return await _context.Set<TransferEntity>()
            .Where(x => x.IdHospitalFrom == hospitalId 
                     || x.IdHospitalTo == hospitalId)
            .ToListAsync();
    }

    public async Task<TransferRequestEntity?> 
        GetTransferRequestByIdAsync(Guid id)
    {
        return await _context.Set<TransferRequestEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> 
        UpdateTransferRequestAsync(TransferRequestEntity entity)
    {
        _context.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
