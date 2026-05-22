using Core.Domain.Commons;
using System.Data;
using System.Security;

namespace Core.Domain.Entity.SEIH;

public class RolePermissionEntity: AuditableEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
