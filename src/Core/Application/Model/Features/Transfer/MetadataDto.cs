namespace Core.Application.Model.Features.Transfer;

public class MetadataDto
{
    public string PayloadHash { get; set; } = "";
    public string EncryptedKey { get; set; } = "";
    public string IV { get; set; } = "";

    public string? Nonce { get; set; }
    public int KeyVersion { get; set; }
}