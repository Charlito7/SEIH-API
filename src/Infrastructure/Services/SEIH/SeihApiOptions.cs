using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.SEIH;

public class SeihApiOptions
{
    public string BaseUrl { get; set; } = "http://localhost:5199";
    public string? BearerToken { get; set; }
}
