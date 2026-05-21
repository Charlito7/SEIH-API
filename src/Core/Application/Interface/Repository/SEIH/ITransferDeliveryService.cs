using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH.Hospital;

public interface ITransferDeliveryService
{
    Task ForwardToTargetHospital(TransferEntity transfer);
}
