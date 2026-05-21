using Microsoft.AspNetCore.Http;

namespace Core.Application.Model.Features;

public class TransferCreateDto
{
    public Guid HospitalToId { get; set; }
    public string EncryptedPayload { get; set; } = string.Empty;
    public string EncryptedKey { get; set; } = string.Empty;
    public string IV { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string PayloadHash { get; set; } = string.Empty;
    public long PayloadSize { get; set; }
    public string PayloadType { get; set; } = string.Empty;
    public string SchemaVersion { get; set; } = string.Empty;
    public Guid ConsentId { get; set; }
    public string ConsentHash { get; set; } = string.Empty;
    public DateTime? ConsentExpiration { get; set; }
    public string PatientReference { get; set; } = string.Empty;
}
