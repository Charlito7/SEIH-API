using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Constants;

public class InfrastructureConstants
{
    public static readonly double TOKEN_EXPIRY_DURATION_MINUTES = 60;
    public static readonly double TOKEN_EXPIRY_DURATION_DAYS = 5;
    public static readonly string DB_CONTEXT_NAME = "ApplicationDbContext";
    public static readonly string ASP_NET_CORE_ENVIRONMENT_NAME = "ASPNETCORE_ENVIRONMENT";
    public static readonly string DEV_ENVIRONMENT_NAME = "Development";
    public static readonly string CONSUMBALE_S3_FOLDER_NAME = "consumables";
}
