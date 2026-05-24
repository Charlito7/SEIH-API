using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        {
            var allowedIps = _config
                .GetSection("AllowedIPs")
                .Get<string[]>() ?? Array.Empty<string>();

            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress?.IsIPv4MappedToIPv6 == true)
            {
                remoteIpAddress = remoteIpAddress.MapToIPv4();
            }

            var remoteIp = remoteIpAddress?.ToString();

            if (!allowedIps.Any())
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("IP whitelist not configured.");
                return;
            }

            if (string.IsNullOrWhiteSpace(remoteIp) || !allowedIps.Contains(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"IP not allowed: {remoteIp}");
                return;
            }

            await _next(context);
        }

    }
}