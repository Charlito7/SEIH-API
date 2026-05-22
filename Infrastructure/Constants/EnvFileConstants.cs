using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Constants;

public class EnvFileConstants
{
    public static readonly string CONNECTION_STRING = "CONNECTION_STRING";

    public static readonly string ACCESS_TOKEN_SECRET = "ACCESS_TOKEN_SECRET";

    public static readonly string REFRESH_TOKEN_SECRET = "REFRESH_TOKEN_SECRET";

    public static readonly string ISSUER = "ISSUER";

    public static readonly string AUDIENCE = "AUDIENCE";

    public static readonly string STAGING_ENV_FILE_NAME = ".env_staging";

    public static readonly string DEV_ENV_FILE_NAME = ".env";

    public static readonly string MICROSOFT_CLIENT_ID = "MICROSOFT_CLIENT_ID";

    public static readonly string MICROSOFT_CLIENT_SECRET = "MICROSOFT_CLIENT_SECRET";

    public static readonly string EMAIL_DO_NOT_REPLY = "EMAIL_DO_NOT_REPLY";

    public static readonly string EMAIL_DO_NOT_REPLY_KEY = "EMAIL_DO_NOT_REPLY_KEY";
    
    public static readonly string AWS_ACCESS_KEY_ID = "AWS_ACCESS_KEY_ID";
    
    public static readonly string AWS_SECRET_ACCESS_KEY = "AWS_SECRET_ACCESS_KEY";

    public static readonly string AWS_CONSUMABLES_BUCKET_NAME = "AWS_CONSUMABLES_BUCKET_NAME";

    public static readonly string AWS_CONSUMABLES_BUCKET_PREFIX = "AWS_CONSUMABLES_BUCKET_PREFIX";

    public static readonly string ACCESS_TOKEN_LIFETIME_IN_DAYS = "ACCESS_TOKEN_LIFETIME_IN_DAYS";

    public static readonly string TEMP_FILE = "temp.txt";
    public static readonly string HOST = "HOST";
    public static readonly string PORT = "PORT";
    public static readonly string SCHEME = "SCHEME";
    public static readonly string RETURN_URL = "RETURN_URL";
    public static readonly string EXTERNAL_LOGIN_CALLBACK_ROUTE = "EXTERNAL_LOGIN_CALLBACK_ROUTE";
    public static readonly string FRONT_END_HOST = "FRONT_END_HOST";
    public static readonly string? REVERSE_PROXY;

    
}
