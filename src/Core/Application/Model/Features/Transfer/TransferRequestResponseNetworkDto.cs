using Core.Domain.Entity.SEIH;

namespace Core.Application.Model.Features.Transfer;

public class TransferRequestResponseNetworkDto
{
    public Guid RequestId { get; set; }
    public TransferRequestStatus Status { get; set; }
    public string? ResponseReason { get; set; }
    public Guid? TransferId { get; set; }
}