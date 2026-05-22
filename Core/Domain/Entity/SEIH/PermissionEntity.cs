using Core.Domain.Commons;

namespace Core.Domain.Entity.SEIH;

public class PermissionEntity : AuditableEntity
{
    public string? HttpMethod { get; set; }  
    public string? Path { get; set; }
    public string? Name { get; set; }
    public bool IsCreatedBySEIH { get; set; }
}
