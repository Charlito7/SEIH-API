using Core.Domain.Entity.SEIH;

namespace Core.Application.Model.Features;

public class TransferRequestNetworkDto
{
    public Guid RequestId { get; set; }
    public Guid HospitalFromId { get; set; }
    public Guid HospitalToId { get; set; }
    public string InfoPatient { get; set; } = string.Empty;
    public TransferRequestStatus Status { get; set; }
    public Guid? TransferId { get; set; }
    public Guid ConsentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? RequestReason { get; set; }
    public string? ResponseReason { get; set; }
}
