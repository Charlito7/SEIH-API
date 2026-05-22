using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH;

public interface IInstitutionKeyRepository
{
    Task AddAsync(InstitutionKeyEntity entity);
    Task UpdateAsync(InstitutionKeyEntity entity);
    Task<IEnumerable<InstitutionKeyEntity>> GetByHospitalAsync(Guid hospitalId);
    Task<InstitutionKeyEntity?> GetByHospitalAndVersionAsync(Guid hospitalId, int keyVersion);
}
