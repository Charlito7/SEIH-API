namespace Core.Domain.Procedures.SEIH;

public class GetUserRolesResponse
{
    public string RoleName { get; set; } = string.Empty;
}


public class GetUserRolesWithPermissionResponse
{
    public Guid? RoleId { get; set; }
    public Guid? RoleHospitalId { get; set; }
    public Guid? PermissionId { get; set; }
    public string? RoleName { get; set; } = string.Empty;
    public string? HttpMethod { get; set; } = string.Empty;
    public string? Path { get; set; } = string.Empty;
    public string? PermissionName { get; set; } = string.Empty;

}
