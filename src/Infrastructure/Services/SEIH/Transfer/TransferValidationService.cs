using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Core.Domain.Entity.SEIH;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services.SEIH.Transfer;

public class TransferValidationService : ITransferValidationService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<TransferValidationService> _logger;

    public TransferValidationService(AppDbContext context, IConfiguration config,
    ILogger<TransferValidationService> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    public async Task<ServiceResult<TransferEntity>> ValidateAndStoreAsync(TransferReceiveDto dto)
    {
        var encryptedPayload = Convert.FromBase64String(dto.EncryptedPayload);
        var encryptedKey = Convert.FromBase64String(dto.EncryptedKey);
        var iv = Convert.FromBase64String(dto.IV);

        // 1️⃣ Validate Hash changé temporairement
        // if (!ValidateHash(encryptedPayload, dto.PayloadHash))
        //     return new ServiceResult<bool>(false, false, HttpStatusCode.BadRequest, "Invalid hash");
        // 1️⃣ Validate Hash (désactivé temporairement pour Swagger tests)
        var disableHash = _config.GetValue<bool>("Security:DisableHashValidation");

        if (!disableHash)
        {
            if (!ValidateHash(encryptedPayload, dto.PayloadHash))
                return new ServiceResult<TransferEntity>(new TransferEntity(), false, HttpStatusCode.BadRequest, "Invalid hash");
            // return new ServiceResult<bool>(false, false, HttpStatusCode.BadRequest, "Invalid hash");
        }
        //fin



        // 2️⃣ Validate Nonce
        if (await _context.UsedNonces.AnyAsync(x => x.Value == dto.Nonce))
            return new ServiceResult<TransferEntity>(new TransferEntity(), false, HttpStatusCode.BadRequest, "Replay attack detected");

        // 3️⃣ Validate Timestamp
        // if (DateTime.UtcNow - dto.SignedAt > TimeSpan.FromMinutes(5))
        //     return new ServiceResult<bool>(false, false, HttpStatusCode.BadRequest, "Expired signature");
        var disableTimestamp = _config.GetValue<bool>("Security:DisableTimestampValidation");

        if (!disableTimestamp)
        {
            if (DateTime.UtcNow - dto.SignedAt > TimeSpan.FromMinutes(5))
                return new ServiceResult<TransferEntity>(new TransferEntity(), false, HttpStatusCode.BadRequest, "Expired signature");
        }
        //fin


        Console.WriteLine($"HospitalFrom: {dto.IdHospitalFrom}");
        Console.WriteLine($"KeyVersion: {dto.KeyVersion}");

        _logger.LogInformation(
            "VALIDATING KEY → Hospital:{HospitalId} Version:{KeyVersion}",
            dto.IdHospitalFrom,
            dto.KeyVersion
        );

        // 4️⃣ Retrieve source public key
        var key = await _context.TransferInstitutionKeys
            .FirstOrDefaultAsync(x =>
                x.HospitalId == dto.IdHospitalFrom &&
                x.KeyVersion == dto.KeyVersion &&
                x.IsActive);

        if (key == null)
            return new ServiceResult<TransferEntity>(new TransferEntity(), false, HttpStatusCode.BadRequest, "Invalid key version");

        var signedData = Encoding.UTF8.GetBytes(dto.SignedData);

        _logger.LogInformation("SIGNED DATA HUB: {data}", dto.SignedData);

        // 6️⃣ Verify Signature

        var disableSignature = _config.GetValue<bool>("Security:DisableSignatureValidation");

        _logger.LogError("🚨 CONFIG SIGNATURE = {value}", disableSignature);

        if (!disableSignature)
        {
            if (!VerifySignature(signedData, dto.Signature, key.PublicKey))
                return new ServiceResult<TransferEntity>(new TransferEntity(), false, HttpStatusCode.BadRequest, "Invalid signature");
        }
        //fin

        _logger.LogWarning("PUBLIC KEY USED FOR SIGNATURE:");
        _logger.LogWarning("{key}", key.PublicKey);




        // 7️⃣ Store nonce
        _context.UsedNonces.Add(new UsedNonceEntity
        {
            Id = Guid.NewGuid(),
            Value = dto.Nonce,
            CreatedAt = DateTime.UtcNow
        });

        // 8️⃣ Store transfer
        var transfer = new TransferEntity
        {
            Id = Guid.NewGuid(),
            IdHospitalFrom = dto.IdHospitalFrom,
            IdHospitalTo = dto.IdHospitalTo,
            EncryptedPayload = encryptedPayload,
            EncryptedSessionKey = encryptedKey,
            IV = iv,
            Signature = dto.Signature,
            PayloadHash = dto.PayloadHash,
            PayloadSize = dto.PayloadSize,
            SchemaVersion = dto.SchemaVersion,
            KeyVersion = dto.KeyVersion,
            Nonce = dto.Nonce,
            SignedAt = dto.SignedAt,
            Status = "RECEIVED",
            Message = dto.Message,
            IdConsent = Guid.Empty,
            ConsentHash = "",
            ConsentExpiration = null,
            PatientReference = "",
            SignedData = dto.SignedData,
        };

        _context.Transfers.Add(transfer);

        await _context.SaveChangesAsync();

        return new ServiceResult<TransferEntity>(result: transfer, isOk: true, status: HttpStatusCode.OK);
    }

    private bool ValidateHash(byte[] encryptedPayload, string expectedHash)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(encryptedPayload);
        return Convert.ToBase64String(hash) == expectedHash;
    }

    private bool VerifySignature(byte[] signedData, string signatureBase64, string publicKeyPem)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);

        return rsa.VerifyData(
            signedData,
            Convert.FromBase64String(signatureBase64),
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
    }
}


