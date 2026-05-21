using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Core.Domain.Entity.SEIH;
using System.Net;
using System.Security.Claims;

using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.SEIH.Transfer;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _repository;
    private readonly IUsersRepository _usersRepository;
    private readonly IHospitalRepository _hospitalRepository;

    public TransferService(
    ITransferRepository repository,
    IUsersRepository usersRepository,
    IHospitalRepository hospitalRepository)
    {
        _repository = repository;
        _usersRepository = usersRepository;
        _hospitalRepository = hospitalRepository;
    }

    public async Task<ServiceResult<bool>> StartAsync(
    Guid sourceHospitalId,
    TransferStartRequestDto request)
    {
        // 🔎 1️⃣ Vérifier hôpital destination
        var hospitalTo = await _hospitalRepository
            .GetHospitalByIdAsync(request.DestinationHospitalId);

        if (hospitalTo == null)
            return new ServiceResult<bool>(HttpStatusCode.BadRequest);

        // 🔍 2️⃣ Vérification intégrité payload
        var computedHash = ComputeSHA256(request.EncryptedPayload);

        if (computedHash != request.PayloadHash)
            return new ServiceResult<bool>(HttpStatusCode.BadRequest);

        // 🔍 3️⃣ Vérification expiration consent
        if (request.ConsentExpiration != null &&
            request.ConsentExpiration < DateTime.UtcNow)
            return new ServiceResult<bool>(HttpStatusCode.BadRequest);

        // 📦 4️⃣ Création du transfert
        var transfer = new TransferEntity
        {
            Id = Guid.NewGuid(),

            // 🔐 SOURCE vient du certificat
            IdHospitalFrom = sourceHospitalId,

            IdHospitalTo = request.DestinationHospitalId,

            EncryptedPayload = Convert.FromBase64String(request.EncryptedPayload),
            EncryptedSessionKey = Convert.FromBase64String(request.EncryptedKey),
            IV = Convert.FromBase64String(request.IV),

            Signature = request.Signature,

            PayloadHash = request.PayloadHash,
            PayloadSize = request.PayloadSize,
            PayloadType = request.PayloadType,
            SchemaVersion = request.SchemaVersion,

            IdConsent = request.ConsentId,
            ConsentHash = request.ConsentHash,
            ConsentExpiration = request.ConsentExpiration,
            Nonce = request.Nonce,
            SignedAt = request.SignedAt,
            KeyVersion = request.KeyVersion,

            PatientReference = request.PatientReference,

            Status = "RECEIVED",
            Created = DateTime.UtcNow,
            CreatedBy = sourceHospitalId.ToString(),
            IsDeleted = false
        };

        var created = await _repository.CreateTransferAsync(transfer);

        if (!created)
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> CreateTransferAsync(
        ClaimsPrincipal claim,
        TransferCreateDto request)
    {
        var email = claim.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _usersRepository.GetUserByEmailAsync(email!);

        if (user == null)
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);

        var hospitalTo = await _hospitalRepository.GetHospitalByIdAsync(request.HospitalToId);

        if (hospitalTo == null)
            return new ServiceResult<bool>(HttpStatusCode.BadRequest);

        // 🔍 1️⃣ Vérification intégrité payload
        var computedHash = ComputeSHA256(request.EncryptedPayload);

        if (computedHash != request.PayloadHash)
            return new ServiceResult<bool>(HttpStatusCode.BadRequest);

        // 🔍 2️⃣ Vérification expiration consent
        if (request.ConsentExpiration != null &&
            request.ConsentExpiration < DateTime.UtcNow)
            return new ServiceResult<bool>(HttpStatusCode.BadRequest);

        var transfer = new TransferEntity
        {
            Id = Guid.NewGuid(),
            IdHospitalFrom = user.HospitalId,
            IdHospitalTo = request.HospitalToId,

            EncryptedPayload = Convert.FromBase64String(request.EncryptedPayload),
            EncryptedSessionKey = Convert.FromBase64String(request.EncryptedKey),
            IV = Convert.FromBase64String(request.IV),

            Signature = request.Signature,
            PayloadHash = request.PayloadHash,
            PayloadSize = request.PayloadSize,
            PayloadType = request.PayloadType,
            SchemaVersion = request.SchemaVersion,

            IdConsent = request.ConsentId,
            ConsentHash = request.ConsentHash,
            ConsentExpiration = request.ConsentExpiration,

            PatientReference = request.PatientReference,

            Status = "RECEIVED",
            Created = DateTime.UtcNow,
            CreatedBy = user.Id.ToString(),
            IsDeleted = false
        };

        var created = await _repository.CreateTransferAsync(transfer);

        if (!created)
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);

        return new ServiceResult<bool>(true);
    }

    private string ComputeSHA256(string base64Payload)
    {
        var bytes = Convert.FromBase64String(base64Payload);
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<ServiceResult<IEnumerable<TransferDto>>>
        GetTransferListAsync(ClaimsPrincipal claim)
    {
        var email = claim.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _usersRepository.GetUserByEmailAsync(email!);

        if (user == null)
            return new ServiceResult<IEnumerable<TransferDto>>(HttpStatusCode.Unauthorized);

        var transfers = await _repository.GetTransfersByHospitalAsync(user.HospitalId);

        var result = transfers.Select(t => new TransferDto
        {
            Id = t.Id!.Value,
            IdHospitalFrom = t.IdHospitalFrom,
            IdHospitalTo = t.IdHospitalTo,
            EncryptedPayload = Convert.ToBase64String(t.EncryptedPayload ?? Array.Empty<byte>())
        });

        return new ServiceResult<IEnumerable<TransferDto>>(result);
    }





}
