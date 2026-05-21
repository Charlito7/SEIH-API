using Core.Domain.Commons;

namespace Core.Domain.Entity.SEIH;

public class TransferEntity : AuditableEntity
{
    public Guid IdHospitalFrom { get; set; }
    public Guid IdHospitalTo { get; set; }
    public byte[]? EncryptedPayload { get; set; }
    public byte[]? EncryptedSessionKey { get; set; }
    public byte[]? IV { get; set; }
    public string? Signature { get; set; }
    public string? PayloadHash { get; set; }
    public string? SignedData { get; set; }
    public int? KeyVersion { get; set; }
    public string? Nonce { get; set; }
    public DateTime? SignedAt { get; set; }
    public long PayloadSize { get; set; }
    public string PayloadType { get; set; } = "SEIH_PACKAGE";
    public string SchemaVersion { get; set; } = "SEIH-1.0";
    public Guid? IdConsent { get; set; }
    public string? ConsentHash { get; set; }
    public DateTime? ConsentExpiration { get; set; }
    public string PatientReference { get; set; } = string.Empty;
    public string Status { get; set; } = "CREATED";
    public string? Message { get; set; }
}