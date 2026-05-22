using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using System.Security.Claims;
using Core.Application.Model.Features.Hospital;
using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Services.SEIH;

public interface IHospitalService
{
    Task<ServiceResult<List<HospitalDto>>> GetAllAsync();
    Task<ServiceResult<HospitalDto?>> GetByIdAsync(Guid id);
    Task<ServiceResult<bool>> AddAsync(HospitalDto dto);
    // Task<ServiceResult<bool>> UpdateAsync(HospitalDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> ActivateAsync(Guid id);
    Task<ServiceResult<bool>> DeactivateAsync(Guid id);
    Task<ServiceResult<string?>> GetPublicKeyAsync(Guid hospitalId);
    Task<ServiceResult<RegisterPublicKeyResponse>> RegisterPublicKeyAsync(ClaimsPrincipal claim, string publicKey);
    Task<bool> IsCertificateValidAsync(string thumbprint);
    Task<ServiceResult<IEnumerable<PublicHospitalDto>>> GetPublicHospitalsAsync();
    Task<bool> IsApiKeyValidAsync(string apiKey);
    Task<HospitalEntity?> GetHospitalByApiKeyAsync(string apiKey);
    Task<ServiceResult<bool>> RegisterTransferKeyAsync(RegisterTransferKeyRequest request);
    Task<List<HospitalWithKeysDto>> GetNetworkHospitalsAsync();
    Task<ServiceResult<CreateHospitalResponse>> CreateWithSecurityAsync(CreateHospitalRequest request);
    Task<ServiceResult<bool>> UpdateAsync(UpdateHospitalRequest request);
}
