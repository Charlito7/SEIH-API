namespace Core.Domain.Commons;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? LastDeleted { get; set; }
    public string? LastDeletedBy { get; set; }
}