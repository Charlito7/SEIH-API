
namespace Core.Application.Model.Features;

public class HospitalRoleDto
{
    public string? RoleName { get; set; }
}

public class HospitalRolePermissionDto
{
    public string? RoleName { get; set; }
    public List<string>? PermissionName { get; set; }
}
