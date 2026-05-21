using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH.Hospital;

public interface ITransferInstitutionKeyRepository
{
   Task AddAsync(TransferInstitutionKeyEntity entity);

    Task UpdateAsync(TransferInstitutionKeyEntity entity);

    Task<IEnumerable<TransferInstitutionKeyEntity>> GetByHospitalAsync(Guid hospitalId);

    Task<TransferInstitutionKeyEntity?> GetActiveByHospitalAsync(Guid hospitalId);

    Task<TransferInstitutionKeyEntity?> GetByHospitalAndVersionAsync(Guid hospitalId, int keyVersion);
}
