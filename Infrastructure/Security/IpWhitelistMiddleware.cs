using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.Application.Interface.Repository.SEIH;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Core.Application.Interface.Services.SEIH;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security
{
    public class IpWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly ILogger<IpWhitelistMiddleware> _logger;

        public IpWhitelistMiddleware(
            RequestDelegate next,
            IConfiguration config,
            ILogger<IpWhitelistMiddleware> logger)
        {
            _next = next;
            _config = config;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {Console.WriteLine("IP WHITELIST START");
            var allowedIps = _config
                .GetSection("AllowedIPs")
                .Get<string[]>() ?? Array.Empty<string>();

            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            _logger.LogInformation("IP received: {IP}", remoteIp);
            _logger.LogInformation("Allowed IPs: {Allowed}", string.Join(",", allowedIps));

            if (!allowedIps.Any())
            {
                _logger.LogWarning("Allowed IP list is empty.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("IP whitelist not configured.");
                return;
            }

            if (string.IsNullOrWhiteSpace(remoteIp) || !allowedIps.Contains(remoteIp))
            {
                _logger.LogWarning("IP not allowed: {IP}", remoteIp);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("IP not allowed.");
                return;
            }
Console.WriteLine("IP PASSED");
            await _next(context);


        }
    }
}