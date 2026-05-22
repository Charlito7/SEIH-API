namespace Core.Application.Model.Features.Hospital
{
    public class RegisterTransferKeyRequest
{
    public Guid HospitalId { get; set; }

    public string PublicKey { get; set; } = string.Empty;

    public int KeyVersion { get; set; }
}
}
