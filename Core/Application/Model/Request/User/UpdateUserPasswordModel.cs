using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request;

public class UpdateUserPasswordModel
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? OldPassword { get; set; }
    [Required]
    public string? NewPassword { get; set; }
}

public class UpdateUserPasswordByManagerModel
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? NewPassword { get; set; }
}