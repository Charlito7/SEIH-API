using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features.Hospital;

namespace Core.Application.Interface.Security;

public interface IInstitutionKeyService
{
    Task<ServiceResult<bool>> RotateKeyAsync(RotateKeyDto dto);
    Task<ServiceResult<bool>> RevokeKeyAsync(Guid hospitalId, int keyVersion);
    Task<ServiceResult<bool>> ValidateKeyVersionAsync(Guid hospitalId, int keyVersion);
    Task<ServiceResult<IEnumerable<KeyInfoDto>>> GetHospitalKeysAsync(Guid hospitalId);
    Task<string?> GetActivePublicKeyAsync(Guid hospitalId, int keyVersion);
}
