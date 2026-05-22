using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH;

public interface IHospitalRepository
{
    Task<List<HospitalEntity>> GetAllAsync();
    Task<HospitalEntity?> GetHospitalByNameAsync(string hospitalName);
    Task<HospitalEntity?> GetHospitalByIdAsync(Guid hospitalId);
    Task AddAsync(HospitalEntity hospital);
    Task UpdateAsync(HospitalEntity hospital);
    Task DeleteAsync(Guid id);
    Task<HospitalEntity?> GetByIdAsync(Guid id);
    Task<HospitalEntity?> GetByThumbprintAsync(string thumbprint);
    Task<HospitalEntity?> GetByApiKeyAsync(string apiKey);
    Task<HospitalEntity?> GetByCertificateThumbprintAsync(string thumbprint);
}
