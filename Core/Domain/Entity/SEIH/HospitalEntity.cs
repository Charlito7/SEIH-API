using Core.Domain.Commons;

namespace Core.Domain.Entity.SEIH
{
    public class HospitalEntity : AuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string City { get; set; } = default!;
        public string Department { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string? PublicKey { get; set; }
        public DateTime? PublicKeyCreateDate { get; set; }
        public DateTime? PublicKeyExpireDate { get; set; }
        public string? PublicKeyFingerprint { get; set; }
        public string? ApiKey { get; set; }
        public DateTime? ApiKeyExpireDate { get; set; }
        public string? CertificateThumbprint { get; set; }
        public bool IsCertificateRevoked { get; set; } = false;
        public HospitalIntegrationType IntegrationType { get; set; }
    }
}
