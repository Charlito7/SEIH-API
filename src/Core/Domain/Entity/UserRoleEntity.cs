using Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;


namespace Core.Domain.Entities;

public class UserRoleEntity : IdentityRole
{
    public UserRoleEntity() { }

    //Creating User Roles
    public UserRoleEntity(UserRoleEnums role) : base(role.ToString()!) { }
    public UserRoleEntity(string role) : base(role) { }
    public Guid HospitalId { get; set; }
}
