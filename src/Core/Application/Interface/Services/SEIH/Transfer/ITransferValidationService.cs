using Core.Application.Commons.ServiceResult;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Core.Domain.Entity.SEIH;
using System.Security.Claims;

namespace Core.Application.Interface.Services.SEIH.Transfer;

public interface ITransferValidationService
{
    Task<ServiceResult<TransferEntity>> ValidateAndStoreAsync(TransferReceiveDto dto);
}
