using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH.Transfer;

public interface ITransferService
{
    Task<ServiceResult<bool>> CreateTransferAsync(ClaimsPrincipal claim,TransferCreateDto request);
    Task<ServiceResult<IEnumerable<TransferDto>>>GetTransferListAsync(ClaimsPrincipal claim);
    Task<ServiceResult<bool>> StartAsync(Guid sourceHospitalId, TransferStartRequestDto request);
}
