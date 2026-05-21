namespace Core.Domain.Procedures.SEIH;

public class GetUserListWithRolesResponse
{
    public Guid? UserId { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime Created { get; set; }
    public string? Roles { get; set; } = string.Empty;
}
