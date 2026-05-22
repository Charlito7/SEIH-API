using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response;

public class GetProfileDataResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get { return $"{FirstName} {LastName}"; } }
    public string? UserRole { get; set; }
    public string? Email { get; set; }
}
