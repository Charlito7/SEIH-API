using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Core.Domain.Entity.SEIH;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;

using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.SEIH.Transfer;

// Infrastructure.Services.SEIH.Transfer

public class TransferDeliveryService : ITransferDeliveryService
{
    private readonly ILogger<TransferDeliveryService> _logger;

    public TransferDeliveryService(ILogger<TransferDeliveryService> logger)
    {
        _logger = logger;
    }

    public async Task ForwardToTargetHospital(TransferEntity transfer)
    {
        _logger.LogInformation("FORWARDING TRANSFER {id} TO TARGET HOSPITAL {to}",
            transfer.Id, transfer.IdHospitalTo);

        // 👉 Pour l’instant juste log (on fera HTTP après)
        await Task.CompletedTask;
    }
}