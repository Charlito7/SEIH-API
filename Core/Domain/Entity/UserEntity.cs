using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class UserEntity : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string? Id { get; set; }
    public override string? UserName { get; set; }
    public override string? Email { get; set; }
    public override string? NormalizedUserName { get; set; }
    public override string? NormalizedEmail { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? Password { get; set; }
    // Salt properties
    public byte[]? Salt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    //Hospital
    public Guid HospitalId { get; set; }

    //auditable
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsNewPasswordRequired { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }

    // Note: overriding properties in IdentityUser base class, so that EF generates the columns in right order
    public virtual DateTimeOffset? LockoutEnd { get; set; }
    public override bool TwoFactorEnabled { get; set; }
    public override bool PhoneNumberConfirmed { get; set; }
    public override string? PhoneNumber { get; set; }
    public virtual string? ConcurrencyStamp { get; set; }
    public override string? SecurityStamp { get; set; }
    public override string? PasswordHash { get; set; }
    public override bool EmailConfirmed { get; set; }
    public override bool LockoutEnabled { get; set; }
    public override int AccessFailedCount { get; set; }
    
}
