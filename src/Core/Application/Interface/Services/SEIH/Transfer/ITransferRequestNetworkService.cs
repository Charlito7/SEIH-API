using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH.Transfer;

public interface ITransferRequestNetworkService
{
    Task<bool> ReceiveAsync(TransferRequestNetworkDto dto);
    Task<IEnumerable<TransferRequestNetworkDto>>GetIncomingAsync(Guid hospitalId);
    Task<bool> UpdateStatusAsync(TransferRequestResponseNetworkDto dto);
}
