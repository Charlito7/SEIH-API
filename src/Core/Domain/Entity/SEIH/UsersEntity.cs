using Core.Domain.Commons;

namespace Core.Domain.Entity.SEIH;

public class UsersEntity: AuditableEntity
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? IsNewPasswordRequired { get; set; }
    public byte[]? Salt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public  string? PasswordHash { get; set; }
    public Guid HospitalId { get; set; }
}

