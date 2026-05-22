using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH;

public interface ITransferRequestRepository
{
    Task<bool> CreateAsync(TransferRequestEntity entity);
    Task<IEnumerable<TransferRequestEntity>> GetHospitalRequestsAsync(Guid hospitalId);
    Task<TransferRequestEntity?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(TransferRequestEntity entity);
}
