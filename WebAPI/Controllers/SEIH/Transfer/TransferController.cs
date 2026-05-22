using System.Text;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Core.Domain.Entity.SEIH;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers.SEIH.HOSPITALMANAGER;

[ApiController]
[Route("api/rest/seih/transfer")]
[AllowAnonymous]
public class TransferController : ControllerBase
{
    private readonly ITransferValidationService _validationService;
    private readonly AppDbContext _context;
    private readonly ILogger<TransferController> _logger;

    private readonly ITransferDeliveryService _deliveryService;

    public TransferController(
        ITransferValidationService validationService,
        AppDbContext context,
        ILogger<TransferController> logger,
        ITransferDeliveryService deliveryService) 
    {
        _validationService = validationService;
        _context = context;
        _logger = logger;
        _deliveryService = deliveryService; 
    }

    [HttpPost("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllTransfers()
    {
        try
        {
            var apiKey = Request.Headers["X-API-KEY"].ToString();

            if (string.IsNullOrEmpty(apiKey))
                return Unauthorized("Missing API KEY");

            
            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(h => h.ApiKey == apiKey);

            if (hospital == null)
                return Unauthorized("Invalid API KEY");

            var transfers = await _context.Transfers
                .AsNoTracking()
                .OrderByDescending(t => t.Created)
                .Take(100)
                .ToListAsync();

            var result = transfers.Select(t => new GlobalTransferDto
            {
                Id = t.Id ?? Guid.Empty,

                IdHospitalFrom = t.IdHospitalFrom,
                IdHospitalTo = t.IdHospitalTo,

                Status = t.Status ?? "UNKNOWN",
                Created = t.Created,

                Message = t.Message ?? "",
                PatientReference = t.PatientReference ?? ""
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "FAILED_TO_FETCH_TRANSFERS",
                message = ex.Message
            });
        }
    }

    [HttpPost]
    [Route("/api/rest/transfers")]
    public async Task<IActionResult> Prepare([FromBody] PrepareDto dto)
    {
        var transferId = Guid.NewGuid();
        if (!Guid.TryParse(dto.SenderHospitalId, out var fromId) || !Guid.TryParse(dto.RecipientHospitalId, out var toId))
        {
            return BadRequest("Invalid hospital id format");
        }

        var transfer = new TransferEntity
        {
            Id = transferId,
            IdHospitalFrom = fromId,
            IdHospitalTo = toId,
            Status = "UPLOADING",
            PayloadSize = dto.Size,
            Created = DateTime.UtcNow,
            EncryptedPayload = new byte[1],
        };

        _context.Transfers.Add(transfer);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            transferId = transferId
        });
    }

    [HttpPost("/api/rest/transfers/download")]
    public async Task<IActionResult> Download([FromBody] DownloadRequestDto dto)
    {
        var transfer = await _context.Transfers.FindAsync(dto.TransferId);

        if (transfer == null)
            return NotFound();

        var path = Path.Combine("uploads", dto.TransferId.ToString(), "final.enc");

        if (!System.IO.File.Exists(path))
            return NotFound();

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        return File(stream, "application/octet-stream", "package.enc");
    }

    [HttpPost("/api/rest/transfers/{id}/parts")]
    public async Task<IActionResult> UploadChunk(Guid id,[FromQuery] int part)
    {

        _logger.LogWarning("HUB RECEIVED PART {part} FOR {id}", part, id);
        var transfer = await _context.Transfers.FindAsync(id);

        if (transfer == null)
            return NotFound();

        var folder = Path.Combine("uploads", id.ToString());

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var filePath = Path.Combine(folder, $"part_{part}");

        using var stream = new FileStream(filePath, FileMode.Create);
        await Request.Body.CopyToAsync(stream);

        return Ok(new { status = "PART_RECEIVED", part });
    }

    [HttpPost("/api/rest/transfers/{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var transfer = await _context.Transfers.FindAsync(id);

        if (transfer == null)
            return NotFound();

        var folder = Path.Combine("uploads", id.ToString());
        var finalPath = Path.Combine(folder, "final.enc");

        var parts = Directory.GetFiles(folder)
    .OrderBy(f =>
    {
        var name = Path.GetFileName(f);
        var partStr = name.Replace("part_", "");
        return int.Parse(partStr);
    })
    .ToList();

        using var output = new FileStream(finalPath, FileMode.Create);

        foreach (var part in parts)
        {
            var bytes = await System.IO.File.ReadAllBytesAsync(part);
            await output.WriteAsync(bytes);
        }

        transfer.Status = "RECEIVED";
        await _context.SaveChangesAsync();

        return Ok();
    }



    [HttpPost("incoming")]
    [AllowAnonymous]
    public async Task<IActionResult> GetIncoming()
    {

        var apiKey = Request.Headers["X-API-KEY"].ToString();

        var hospital = await _context.Hospitals
            .FirstOrDefaultAsync(h => h.ApiKey == apiKey);

        if (hospital == null)
            return Unauthorized();

        var hospitalId = hospital.Id;




        // 🔴 1. Récupérer les transferts
        var transfers = await _context.Transfers
            .Where(t => t.IdHospitalTo == hospitalId &&
                        t.Status == "RECEIVED")
            .Take(10) // 🔥 limite pour éviter surcharge
            .ToListAsync();

        // _logger.LogInformation("LOCKED {count} transfers for hospital {id}", transfers.Count, request.HospitalId);
        // _logger.LogInformation("INCOMING QUERY → HospitalId:{id}", request.HospitalId);

        if (!transfers.Any())
            return Ok(new List<IncomingTransferDto>());

        // 🔴 2. LOCK → passer en PROCESSING
        foreach (var t in transfers)
        {
            t.Status = "PROCESSING";
            _logger.LogInformation("TRANSFER FOUND → Id:{id} Status:{status}", t.Id, t.Status);
        }



        await _context.SaveChangesAsync();

        // 🔴 3. Mapper vers DTO
        var result = transfers.Select(t => new IncomingTransferDto
        {
            Id = t.Id!.Value,
            IdHospitalFrom = t.IdHospitalFrom,
            IdHospitalTo = t.IdHospitalTo,
            EncryptedKey = Convert.ToBase64String(t.EncryptedSessionKey ?? Array.Empty<byte>()),
            IV = Convert.ToBase64String(t.IV ?? Array.Empty<byte>()),
            PayloadHash = t.PayloadHash ?? "",
            Nonce = t.Nonce ?? "",
            KeyVersion = t.KeyVersion ?? 0
        }).ToList(); // 🔥🔥🔥 OBLIGATOIRE




        return Ok(result);
    }


    [HttpPost("ack")]
    [AllowAnonymous]
    public async Task<IActionResult> Ack([FromBody] TransferAckDto dto)
    {
        var transfer = await _context.Transfers.FindAsync(dto.TransferId);

        if (transfer == null)
            return NotFound();

        if (transfer.IdHospitalTo != dto.HospitalId)
            return Unauthorized();

        transfer.Status = "COMPLETED";
        transfer.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { status = "ACK_RECEIVED" });
    }


    public class TransferFailDto
    {
        public Guid TransferId { get; set; }
        public Guid HospitalId { get; set; }
    }

    [HttpPost("fail")]
    [AllowAnonymous]
    public async Task<IActionResult> Fail([FromBody] TransferFailDto dto)
    {
        var transfer = await _context.Transfers.FindAsync(dto.TransferId);

        if (transfer == null)
            return NotFound();

        if (transfer.IdHospitalTo != dto.HospitalId)
            return Unauthorized();

        transfer.Status = "FAILED";

        await _context.SaveChangesAsync();

        return Ok(new { status = "FAILED_RECORDED" });
    }


    [HttpPost("/api/rest/transfers/{id}/metadata")]
    public async Task<IActionResult> SaveMetadata(Guid id, [FromBody] MetadataDto dto)
    {
        var transfer = await _context.Transfers.FindAsync(id);

        if (transfer == null)
            return NotFound();

        transfer.PayloadHash = dto.PayloadHash;
        transfer.EncryptedSessionKey = Convert.FromBase64String(dto.EncryptedKey);
        transfer.IV = Convert.FromBase64String(dto.IV);
        transfer.Nonce = dto.Nonce;
        transfer.KeyVersion = dto.KeyVersion;

        await _context.SaveChangesAsync();

        return Ok();
    }



}