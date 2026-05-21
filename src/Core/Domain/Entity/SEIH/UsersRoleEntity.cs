using Core.Domain.Commons;

namespace Core.Domain.Entity.SEIH;

public class UsersRoleEntity: AuditableEntity
{
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
}
