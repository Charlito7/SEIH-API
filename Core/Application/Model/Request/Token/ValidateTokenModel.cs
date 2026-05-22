using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request;

public class ValidateTokenModel
{
    public string? Token { get; set; }
    public string? ClientId { get; set; }
}
