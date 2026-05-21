using Core.Domain.Commons;


namespace Core.Domain.Entity.SEIH
{
    public class InstitutionKeyEntity : AuditableEntity
    {
    public Guid HospitalId { get; set; }
    public string PublicKey { get; set; } = default!;
    public string Fingerprint { get; set; } = default!;
    public int KeyVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime? RevokedAt { get; set; }
    }
}
