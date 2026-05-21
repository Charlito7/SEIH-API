using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Services.SEIH;
using Core.Application.Model.Features;
using Core.Domain.Entity.SEIH;
using System.Net;
using Core.Application.Interface.Repository.SEIH.Hospital;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Application.Model.Features.Hospital;


namespace Infrastructure.Services.SEIH.Hospital;

public class HospitalService : IHospitalService
{

    private readonly IHospitalRepository _hospitalRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IInstitutionKeyRepository _institutionKeyRepository;
    private readonly ITransferInstitutionKeyRepository _transferInstitutionKeyRepository;

    private readonly AppDbContext _context;

    public HospitalService(
        IHospitalRepository hospitalRepository,
        IUsersRepository usersRepository,
        IInstitutionKeyRepository institutionKeyRepository,
        ITransferInstitutionKeyRepository transferInstitutionKeyRepository,
    AppDbContext context)
    {
        _hospitalRepository = hospitalRepository;
        _usersRepository = usersRepository;
        _institutionKeyRepository = institutionKeyRepository;
        _transferInstitutionKeyRepository = transferInstitutionKeyRepository;
        _context = context;
    }

    public async Task<ServiceResult<bool>> AddAsync(HospitalDto dto)
    {
        var entity = new HospitalEntity
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Address = dto.Address,
            Email = dto.Email,
            Department = dto.Department,
            PhoneNumber = dto.PhoneNumber
        };
        try
        {
            await _hospitalRepository.AddAsync(entity);
            return new ServiceResult<bool>(true, true, HttpStatusCode.OK, "");
        }
        catch
        {
            return new ServiceResult<bool>(false, false, HttpStatusCode.BadRequest, "");
        }

    }

    public async Task<ServiceResult<CreateHospitalResponse>> CreateWithSecurityAsync(CreateHospitalRequest request)
    {
        // 🔐 1. Validation clé publique
        try
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(request.PublicKey.ToCharArray());
        }
        catch
        {
            return new ServiceResult<CreateHospitalResponse>(HttpStatusCode.BadRequest);
        }

        // 🔐 2. Générer API KEY sécurisée
        var apiKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        // 🔐 3. Fingerprint
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(request.PublicKey));
        var fingerprint = Convert.ToHexString(hash);
        if (request.Id == Guid.Empty)
            return new ServiceResult<CreateHospitalResponse>(new CreateHospitalResponse(), true, HttpStatusCode.BadRequest, "Id is required");
        var existing = await _hospitalRepository.GetByIdAsync(request.Id);

        if (existing != null)
            return new ServiceResult<CreateHospitalResponse>(
                new CreateHospitalResponse(),
                true,
                HttpStatusCode.Conflict,
                "Hospital already exists"
            );

        // 🔐 4. Création hospital
        var hospital = new HospitalEntity
        {
            Id = request.Id,
            Name = request.Name,
            Code = request.Code,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            Department = request.Department,
            IsActive = true,
            ApiKey = apiKey,
            ApiKeyExpireDate = DateTime.UtcNow.AddYears(1),
            PublicKey = request.PublicKey,
            PublicKeyCreateDate = DateTime.UtcNow,
            PublicKeyExpireDate = DateTime.UtcNow.AddYears(2),
            PublicKeyFingerprint = fingerprint
        };

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _hospitalRepository.AddAsync(hospital);

            var keyEntity = new InstitutionKeyEntity
            {
                Id = Guid.NewGuid(),
                HospitalId = hospital.Id.Value,
                PublicKey = request.PublicKey,
                Fingerprint = fingerprint,
                KeyVersion = 1,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddYears(2),
                IsActive = true

            };

            await _institutionKeyRepository.AddAsync(keyEntity);

            await transaction.CommitAsync();

            return new ServiceResult<CreateHospitalResponse>(
                new CreateHospitalResponse
                {
                    HospitalId = hospital.Id.Value,
                    ApiKey = apiKey,
                    PublicKeyFingerprint = fingerprint
                });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }



    public async Task<ServiceResult<bool>> UpdateAsync(UpdateHospitalRequest request)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(request.Id);

        if (hospital == null)
            return new ServiceResult<bool>(HttpStatusCode.NotFound);

        hospital.Name = request.Name;
        hospital.Code = request.Code;
        hospital.City = request.City;
        hospital.Department = request.Department;
        hospital.Address = request.Address;
        hospital.Email = request.Email;
        hospital.PhoneNumber = request.PhoneNumber;
        hospital.IsActive = request.IsActive;

        await _hospitalRepository.UpdateAsync(hospital);

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
    {
        await _hospitalRepository.DeleteAsync(id);
        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<List<HospitalDto>>> GetAllAsync()
    {
        var hospitals = await _hospitalRepository.GetAllAsync();

        var result = hospitals.Select(h => new HospitalDto
        {
            Id = h.Id!.Value,
            Name = h.Name!,
            Email = h.Email!,
            Code = h.Code!,
            Address = h.Address!,
            City = h.City,
            Department = h.Department,
            PhoneNumber = h.PhoneNumber!,
            IsActive = h.IsActive,
            PublicKey = h.PublicKey,
            PublicKeyCreateDate = h.PublicKeyCreateDate,
            PublicKeyExpireDate = h.PublicKeyExpireDate,
            PublicKeyFingerprint = h.PublicKeyFingerprint
        }).ToList();

        return new ServiceResult<List<HospitalDto>>(result);
    }

    public async Task<ServiceResult<HospitalDto?>> GetByIdAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);

        if (hospital == null)
            return new ServiceResult<HospitalDto?>(HttpStatusCode.NotFound);

        return new ServiceResult<HospitalDto?>(new HospitalDto
        {
            Id = hospital.Id!.Value,
            Name = hospital.Name!,
            Email = hospital.Email!,
            Code = hospital.Code!,
            Address = hospital.Address!,
            City = hospital.City,
            Department = hospital.Department,
            PhoneNumber = hospital.PhoneNumber!,
            PublicKey = hospital.PublicKey,
            PublicKeyCreateDate = hospital.PublicKeyCreateDate,
            PublicKeyExpireDate = hospital.PublicKeyExpireDate,
            PublicKeyFingerprint = hospital.PublicKeyFingerprint
        });
    }

    // public async Task<ServiceResult<bool>> UpdateAsync(HospitalDto dto)
    // {
    //     var hospital = await _hospitalRepository.GetByIdAsync(dto.Id);

    //     if (hospital == null)
    //         return new ServiceResult<bool>(HttpStatusCode.NotFound);

    //     hospital.Name = dto.Name;
    //     hospital.Address = dto.Address;
    //     hospital.Email = dto.Email;
    //     hospital.PhoneNumber = dto.PhoneNumber;
    //     hospital.City = dto.City;
    //     hospital.Department = dto.Department;
    //     hospital.Code = dto.Code;

    //     await _hospitalRepository.UpdateAsync(hospital);

    //     return new ServiceResult<bool>(true);
    // }

    public async Task<bool> IsCertificateValidAsync(string thumbprint)
    {
        var hospital = await _hospitalRepository
            .GetByCertificateThumbprintAsync(thumbprint);

        return hospital != null &&
               hospital.IsActive &&
               !hospital.IsCertificateRevoked;
    }

    public async Task<ServiceResult<string?>> GetPublicKeyAsync(Guid hospitalId)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(hospitalId);

        if (hospital == null || hospital.IsActive == false)
            return new ServiceResult<string?>(HttpStatusCode.NotFound);

        if (hospital.PublicKeyExpireDate is not DateTime expireDate || expireDate <= DateTime.UtcNow)
        {
            return new ServiceResult<string?>(HttpStatusCode.BadRequest);
        }

        return new ServiceResult<string?>(hospital.PublicKey);
    }

    public async Task<ServiceResult<RegisterPublicKeyResponse>> RegisterPublicKeyAsync(ClaimsPrincipal claim, string publicKey)
    {
        var email = claim.FindFirst(ClaimTypes.Email)?.Value;

        var user = await _usersRepository.GetUserByEmailAsync(email!);

        if (user == null)
            return new ServiceResult<RegisterPublicKeyResponse>(HttpStatusCode.Unauthorized);

        var hospitalId = user.HospitalId;

        var existingKeys = (await _institutionKeyRepository.GetByHospitalAsync(hospitalId)).ToList();

        foreach (var k in existingKeys)
        {
            k.IsActive = false;
            await _institutionKeyRepository.UpdateAsync(k);
        }

        var nextVersion = existingKeys.Any()
            ? existingKeys.Max(k => k.KeyVersion) + 1
            : 1;

        using var sha = SHA256.Create();

        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(publicKey));

        var fingerprint = Convert.ToHexString(hash);

        var key = new InstitutionKeyEntity
        {
            Id = Guid.NewGuid(),
            HospitalId = hospitalId,
            PublicKey = publicKey,
            Fingerprint = fingerprint,
            KeyVersion = nextVersion,
            Created = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddYears(2),
            IsActive = true
        };

        await _institutionKeyRepository.AddAsync(key);

        return new ServiceResult<RegisterPublicKeyResponse>(
            new RegisterPublicKeyResponse
            {
                Fingerprint = fingerprint,
                CreatedAt = key.Created
            });
    }

    public async Task<ServiceResult<bool>> ActivateAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);
        if (hospital == null)
            return new ServiceResult<bool>(HttpStatusCode.NotFound);

        hospital.IsActive = true;
        await _hospitalRepository.UpdateAsync(hospital);

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> DeactivateAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);
        if (hospital == null)
            return new ServiceResult<bool>(HttpStatusCode.NotFound);

        hospital.IsActive = false;
        await _hospitalRepository.UpdateAsync(hospital);

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<IEnumerable<PublicHospitalDto>>> GetPublicHospitalsAsync()
    {
        var entities = await _hospitalRepository.GetAllAsync();

        var hospitals = entities
            .Where(h => h.IsActive)
            .Select(h => new PublicHospitalDto
            {
                Id = h.Id!.Value,
                Code = h.Code ?? string.Empty,
                Name = h.Name ?? string.Empty,
                City = h.City,
                Address = h.Address ?? string.Empty,
                Email = h.Email,
                Phone = h.PhoneNumber,
                IsActive = h.IsActive,
                PublicKey = h.PublicKey,
                IntegrationType = h.IntegrationType,
                CreatedAt = h.Created
            });

        return new ServiceResult<IEnumerable<PublicHospitalDto>>(hospitals);
    }
    public async Task<bool> IsApiKeyValidAsync(string apiKey)
    {
        var hospital = await _hospitalRepository.GetByApiKeyAsync(apiKey);

        return hospital != null &&
       hospital.IsActive &&
       hospital.ApiKeyExpireDate.HasValue &&
       hospital.ApiKeyExpireDate.Value > DateTime.UtcNow;
    }

    public async Task<HospitalEntity?> GetHospitalByApiKeyAsync(string apiKey)
    {
        var hospital = await _hospitalRepository.GetByApiKeyAsync(apiKey);

        return hospital != null && hospital.IsActive
            ? hospital
            : null;
    }

    public async Task<ServiceResult<bool>> RegisterTransferKeyAsync(
    RegisterTransferKeyRequest request)
    {
        // 1️⃣ Désactiver les anciennes clés
        var oldKeys = await _transferInstitutionKeyRepository
            .GetByHospitalAsync(request.HospitalId);

        foreach (var k in oldKeys)
        {
            k.IsActive = false;
            await _transferInstitutionKeyRepository.UpdateAsync(k);
        }

        // 2️⃣ Calcul du fingerprint
        using var sha = SHA256.Create();

        var hash = sha.ComputeHash(
            Encoding.UTF8.GetBytes(request.PublicKey));

        var fingerprint = Convert.ToHexString(hash);

        // 3️⃣ Création de la nouvelle clé
        var entity = new TransferInstitutionKeyEntity
        {
            Id = Guid.NewGuid(),

            HospitalId = request.HospitalId,

            PublicKey = request.PublicKey,

            Fingerprint = fingerprint,

            KeyVersion = request.KeyVersion,

            ExpirationDate = DateTime.UtcNow.AddYears(2),

            CreatedAt = DateTime.UtcNow,

            IsActive = true
        };

        // 4️⃣ Enregistrement
        await _transferInstitutionKeyRepository.AddAsync(entity);

        return new ServiceResult<bool>(true);
    }

    public async Task<List<HospitalWithKeysDto>> GetNetworkHospitalsAsync()
    {
        var hospitals = await _hospitalRepository.GetAllAsync();

        var result = new List<HospitalWithKeysDto>();
foreach (var h in hospitals.Where(h => h.IsDeleted != true))
        // foreach (var h in hospitals.Where(h => h.IsActive))
        {
            var keys = await _transferInstitutionKeyRepository
                .GetByHospitalAsync(h.Id!.Value);

            result.Add(new HospitalWithKeysDto
            {
                Id = h.Id.Value,
                Name = h.Name ?? "",
                Code = h.Code ?? "",
                City = h.City,
                Department = h.Department,
                IsActive = h.IsActive,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                Address = h.Address,
                Keys = keys
                    .Where(k => k.IsDeleted != true)
                    .Select(k => new TransferInstitutionKeyDto
                    {
                        PublicKey = k.PublicKey,
                        KeyVersion = k.KeyVersion,
                        ExpirationDate = k.ExpirationDate,
                        IsActive = k.IsActive
                    }).ToList()
            });
        }

        return result;
    }


}

