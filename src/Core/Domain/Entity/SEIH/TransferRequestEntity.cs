using Core.Domain.Commons;

namespace Core.Domain.Entity.SEIH;

public class TransferRequestEntity : AuditableEntity
{
    public string? InfoPatient { get; set; }
    public Guid IdHospitalFrom { get; set; }
    public Guid IdHospitalTo { get; set; }
    public Guid IdConsent { get; set; }
    public TransferRequestStatus Status { get; set; } = TransferRequestStatus.Pending;
    public Guid? TransferId { get; set; }
    public TransferEntity? Transfer { get; set; }
    public string? RequestReason { get; set; }
    public string? ResponseReason { get; set; }
}
