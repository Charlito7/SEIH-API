using Core.Application.Model.Features.Hospital;


namespace Core.Application.Model.Features.Hospital
{
    public class RegisterPublicKeyResponse
    {
        public string Fingerprint { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
