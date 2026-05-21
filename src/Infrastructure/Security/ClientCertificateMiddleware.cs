using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.Application.Interface.Repository.SEIH;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Core.Application.Interface.Services.SEIH;
using System.Security.Cryptography;

namespace Infrastructure.Security
{
    public class ClientCertificateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ClientCertificateMiddleware> _logger;

        public ClientCertificateMiddleware(
            RequestDelegate next,
            ILogger<ClientCertificateMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IHospitalService hospitalService)
        {
            var clientCert = await context.Connection.GetClientCertificateAsync();

            _logger.LogInformation("Remote IP: {IP}", context.Connection.RemoteIpAddress);
            _logger.LogInformation("Client cert null? {IsNull}", clientCert == null);


            if (clientCert == null)
            {
                _logger.LogWarning("No client certificate provided.");
                context.Response.StatusCode = 401;
                return;
            }

            if (!await ValidateCertificateAsync(clientCert, hospitalService))
            {
                _logger.LogWarning("Invalid client certificate.");
                context.Response.StatusCode = 403;
                return;
            }

            await _next(context);
        }

        private async Task<bool> ValidateCertificateAsync(
            X509Certificate2 certificate,
            IHospitalService hospitalService)
        {
            if (DateTime.UtcNow < certificate.NotBefore ||
                DateTime.UtcNow > certificate.NotAfter)
                return false;

            var eku = certificate.Extensions
                .OfType<X509EnhancedKeyUsageExtension>()
                .FirstOrDefault();

            if (eku == null ||
                !eku.EnhancedKeyUsages
                    .Cast<Oid>()
                    .Any(oid => oid.Value == "1.3.6.1.5.5.7.3.2"))
                return false;

            return await hospitalService
                .IsCertificateValidAsync(certificate.Thumbprint);
        }
    }
}