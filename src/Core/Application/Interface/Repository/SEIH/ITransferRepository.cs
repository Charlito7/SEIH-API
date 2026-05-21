using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH.Hospital;

public interface ITransferRepository
{
    Task<bool> CreateTransferAsync(TransferEntity entity);
    Task<IEnumerable<TransferEntity>> GetTransfersByHospitalAsync(Guid hospitalId);
    Task<TransferRequestEntity?> GetTransferRequestByIdAsync(Guid id);
    Task<bool> UpdateTransferRequestAsync(TransferRequestEntity entity);
}
