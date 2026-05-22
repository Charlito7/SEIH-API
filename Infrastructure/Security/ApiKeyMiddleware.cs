using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.Application.Interface.Repository.SEIH;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Core.Application.Interface.Services.SEIH;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Infrastructure.Security
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        public ApiKeyMiddleware(
            RequestDelegate next,
            ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IHospitalService hospitalService)
        {
            Console.WriteLine("APIKEY START");
            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                _logger.LogWarning("API Key missing.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key required.");
                return;
            }
            _logger.LogInformation("API KEY header present: {Present}", context.Request.Headers.ContainsKey("X-API-KEY"));

            var apiKey = extractedApiKey.ToString();
            _logger.LogInformation("API KEY value: {ApiKey}", apiKey);
 

            var isValid = await hospitalService.IsApiKeyValidAsync(apiKey);

            if (!isValid)
            {
                _logger.LogWarning("Invalid or expired API Key.");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid API Key.");
                return;
            }

            _logger.LogInformation("API KEY valid: {IsValid}", isValid);
Console.WriteLine("APIKEY PASSED");
            await _next(context);
        }
    }
}