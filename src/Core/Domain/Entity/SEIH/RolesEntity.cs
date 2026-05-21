using Core.Domain.Commons;
using Core.Domain.Entity.SEIH;

namespace Core.Domain.Entity;

public class RolesEntity : AuditableEntity
{
    public string? Name { get; set; }
    public bool IsBasicRole { get; set; }
    public Guid HospitalId { get; set; }    
}
