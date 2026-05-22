namespace Core.Application.Model.Features.Transfer;

public class IncomingTransferDto
{
    public Guid Id { get; set; }

    public Guid IdHospitalFrom { get; set; }

    public Guid IdHospitalTo { get; set; }

    public string EncryptedPayload { get; set; } = string.Empty;

    public string EncryptedKey { get; set; } = string.Empty;

    public string IV { get; set; } = string.Empty;

    public string Signature { get; set; } = string.Empty;

    public string PayloadHash { get; set; } = string.Empty;

    public int KeyVersion { get; set; }

    public string Nonce { get; set; } = string.Empty;

    public DateTime SignedAt { get; set; }
     public string SignedData { get; set; } = string.Empty;

}