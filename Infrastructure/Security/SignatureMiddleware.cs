using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.Application.Interface.Repository.SEIH;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Core.Application.Interface.Services.SEIH;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using Core.Application.Interface.Security;


namespace Infrastructure.Security
{
    public class SignatureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SignatureMiddleware> _logger;

        public SignatureMiddleware(
            RequestDelegate next,
            ILogger<SignatureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }



        public async Task InvokeAsync(
            HttpContext context,
            IHospitalService hospitalService,
            IInstitutionKeyService keyService)
        {
            Console.WriteLine("SIGNATURE START");

            _logger.LogWarning("🔥 HEADERS RECEIVED:");

foreach (var h in context.Request.Headers)
{
    _logger.LogWarning("{Key} = {Value}", h.Key, h.Value);
}

            // 🔥 AJOUT ICI
            if (context.Request.Path.StartsWithSegments("/api/rest/transfers"))
            {
                Console.WriteLine("SIGNATURE BYPASSED FOR TRANSFERS");
                await _next(context);
                return;
            }

            // 🔥 BYPASS endpoints internes
            // if (context.Request.Path.StartsWithSegments("/api/rest/seih/transfer/incoming") ||
            //     context.Request.Path.StartsWithSegments("/api/rest/seih/transfer/ack") ||
            //     context.Request.Path.StartsWithSegments("/api/rest/seih/transfer/receive"))
            // {
            //     Console.WriteLine("SIGNATURE BYPASSED");
            //     await _next(context);
            //     return;
            // }

            // ===============================
            // 🔐 HEADERS VALIDATION
            // ===============================
            if (!context.Request.Headers.TryGetValue("X-SIGNATURE", out var signatureHeader) ||
                !context.Request.Headers.TryGetValue("X-API-KEY", out var apiKeyHeader) ||
                !context.Request.Headers.TryGetValue("X-TIMESTAMP", out var timestampHeader) ||
                !context.Request.Headers.TryGetValue("X-KEY-VERSION", out var versionHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing security headers.");
                return;
            }

            if (!long.TryParse(timestampHeader, out var timestamp))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid timestamp.");
                return;
            }

            if (!int.TryParse(versionHeader, out var keyVersion))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid key version.");
                return;
            }

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;

            if (Math.Abs((DateTime.UtcNow - requestTime).TotalMinutes) > 5)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Request expired.");
                return;
            }

            var apiKey = apiKeyHeader.ToString();
            var signature = signatureHeader.ToString();

            // ===============================
            // 🏥 AUTH HOSPITAL
            // ===============================
            var hospital = await hospitalService.GetHospitalByApiKeyAsync(apiKey);

            if (hospital == null || !hospital.Id.HasValue)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }

            var hospitalId = hospital.Id.Value;

            _logger.LogInformation("Hospital found: {Hospital}", hospitalId);

            // ===============================
            // 🔑 VALIDATE KEY VERSION
            // ===============================
            var isKeyValid = await keyService.ValidateKeyVersionAsync(hospitalId, keyVersion);

            if (!isKeyValid.IsOk || !isKeyValid.Result)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid or expired key version.");
                return;
            }

            // ===============================
            // 📦 READ BODY (SAFE BINARY)
            // ===============================
            context.Request.EnableBuffering();

            using var ms = new MemoryStream();
            await context.Request.Body.CopyToAsync(ms);

            var bodyBytes = ms.ToArray();

            // 🔥 IMPORTANT → reset stream
            context.Request.Body.Position = 0;

            // ===============================
            // 🔐 HASH (NO DATA EXPOSURE)

            // ===============================
            var hashBytes = SHA256.HashData(bodyBytes);
            var hash = Convert.ToHexString(hashBytes);

            _logger.LogInformation("BODY HASH: {hash}", hash);
            _logger.LogInformation("Body length: {Length}", bodyBytes.Length);

            // ===============================
            // 🔐 SIGNATURE CHECK
            // ===============================
            var payloadToVerify = timestamp + hash;

            var publicKey = await keyService.GetActivePublicKeyAsync(hospitalId, keyVersion);

            if (string.IsNullOrWhiteSpace(publicKey))
            {
                _logger.LogWarning("No public key found for Hospital {HospitalId} version {Version}", hospitalId, keyVersion);

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Public key not found.");
                return;
            }

            var isValid = VerifySignature(payloadToVerify, signature, publicKey);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid signature.");
                return;
            }

            _logger.LogInformation("Signature valid: {Valid}", isValid);

            Console.WriteLine("SIGNATURE PASSED");

            await _next(context);
        }
        private bool VerifySignature(string data, string signatureBase64, string publicKeyPem)
        {
            try
            {
                using var rsa = RSA.Create();

                rsa.ImportFromPem(publicKeyPem.ToCharArray());

                var dataBytes = Encoding.UTF8.GetBytes(data);
                var signatureBytes = Convert.FromBase64String(signatureBase64);

                return rsa.VerifyData(
                    dataBytes,
                    signatureBytes,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);
            }
            catch
            {
                return false;
            }
        }
    }

}