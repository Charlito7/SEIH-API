using Core.Domain.Commons;


namespace Core.Domain.Entity.SEIH
{
    public class TransferInstitutionKeyEntity : AuditableEntity
    {
        public Guid HospitalId { get; set; }
        public string PublicKey { get; set; } = default!;
        public string? Fingerprint { get; set; }
        public int KeyVersion { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
