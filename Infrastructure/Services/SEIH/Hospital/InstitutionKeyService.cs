using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Security;
using Core.Application.Model.Features.Hospital;
using Core.Domain.Entity.SEIH;
using System.Net;

namespace Infrastructure.Services.Security;

public class InstitutionKeyService : IInstitutionKeyService
{
    private readonly IInstitutionKeyRepository _repository;

    public InstitutionKeyService(IInstitutionKeyRepository repository)
    {
        _repository = repository;
    }

    // ===============================
    // 🔁 ROTATION
    // ===============================
    public async Task<ServiceResult<bool>> RotateKeyAsync(RotateKeyDto dto)
    {
        var existingKeys = await _repository.GetByHospitalAsync(dto.HospitalId);

        // Désactiver les clés actives existantes
        foreach (var key in existingKeys.Where(k => k.IsActive))
        {
            key.IsActive = false;
            key.RevokedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(key);
        }

        var newKey = new InstitutionKeyEntity
        {
            Id = Guid.NewGuid(),
            HospitalId = dto.HospitalId,
            PublicKey = dto.PublicKey,
            KeyVersion = dto.KeyVersion,
            Fingerprint = ComputeFingerprint(dto.PublicKey),
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(90), // 🔐 rotation 90 jours
            IsActive = true,
            IsDeleted = false,
            Created = DateTime.UtcNow
        };

        await _repository.AddAsync(newKey);

        return new ServiceResult<bool>(true);
    }

    // ===============================
    // ❌ REVOKE
    // ===============================
    public async Task<ServiceResult<bool>> RevokeKeyAsync(Guid hospitalId, int keyVersion)
    {
        var key = await _repository.GetByHospitalAndVersionAsync(hospitalId, keyVersion);

        if (key == null)
            return new ServiceResult<bool>(HttpStatusCode.NotFound);

        key.IsActive = false;
        key.RevokedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(key);

        return new ServiceResult<bool>(true);
    }

    // ===============================
    // ✅ VALIDATE VERSION
    // ===============================
    public async Task<ServiceResult<bool>> ValidateKeyVersionAsync(Guid hospitalId, int keyVersion)
    {
        var key = await _repository.GetByHospitalAndVersionAsync(hospitalId, keyVersion);

        if (key == null)
            return new ServiceResult<bool>(HttpStatusCode.NotFound);

        if (!key.IsActive)
            return new ServiceResult<bool>(false);

        if (key.ExpirationDate < DateTime.UtcNow)
            return new ServiceResult<bool>(false);

        if (key.RevokedAt != null)
            return new ServiceResult<bool>(false);

        return new ServiceResult<bool>(true);
    }

    // ===============================
    // 📋 LIST KEYS
    // ===============================
    public async Task<ServiceResult<IEnumerable<KeyInfoDto>>> GetHospitalKeysAsync(Guid hospitalId)
    {
        var keys = await _repository.GetByHospitalAsync(hospitalId);

        var result = keys.Select(k => new KeyInfoDto
        {
            KeyVersion = k.KeyVersion,
            CreatedAt = k.CreatedAt,
            ExpirationDate = k.ExpirationDate,
            IsActive = k.IsActive,
            RevokedAt = k.RevokedAt
        });

        return new ServiceResult<IEnumerable<KeyInfoDto>>(result);
    }

    // ===============================
    // 🔐 GET ACTIVE PUBLIC KEY
    // ===============================
    public async Task<string?> GetActivePublicKeyAsync(Guid hospitalId, int keyVersion)
    {
        var key = await _repository.GetByHospitalAndVersionAsync(hospitalId, keyVersion);

        if (key == null)
            return null;

        if (!key.IsActive)
            return null;

        if (key.ExpirationDate < DateTime.UtcNow)
            return null;

        if (key.RevokedAt != null)
            return null;

        return key.PublicKey;
    }

    // ===============================
    // 🔎 FINGERPRINT
    // ===============================
    private string ComputeFingerprint(string publicKey)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(publicKey));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}