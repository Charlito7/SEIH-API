using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request;

public class AddRolesToUserModel
{
    public string? AdminEmail { get; set; }
    public string? Email { get; set; }
    public string[]? Roles { get; set; }
}

public class AddRolesToUserDTO
{
    public string? UserId { get; set; }
    public string? RoleName { get; set; }

}