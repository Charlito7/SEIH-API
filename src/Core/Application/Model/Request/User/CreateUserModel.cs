using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request;

public class CreateUserModel
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    public string? HospitalName { get; set; }
}

public class ChangePasswordModel
{
    [Required]
    public string? OldPassword { get; set; }
    [Required]
    public string? NewPassword { get; set; }

}

public class ChangePasswordByManagerModel
{
    [Required]
    public string? UserEmail { get; set; }
    [Required]
    public string? NewPassword { get; set; }

}