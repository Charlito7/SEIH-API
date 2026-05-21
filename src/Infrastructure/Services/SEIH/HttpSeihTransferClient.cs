using Application.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static Core.Application.Contracts.Transfers;

namespace Infrastructure.Services.SEIH;

public sealed class HttpSeihTransferClient : ISeihTransferClient
{
    private readonly HttpClient _http;
    private readonly ILogger<HttpSeihTransferClient> _logger;

    public HttpSeihTransferClient(HttpClient http, ILogger<HttpSeihTransferClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<CreateTransferResult> PrepareAsync(long fileSize, string recipientHospitalId, CancellationToken ct = default)
    {
        var create = new
        {
            SenderHospitalId = "HOSP-A",
            RecipientHospitalId = recipientHospitalId,
            Size = fileSize,
            ContentType = "application/octet-stream"
        };

        var resp = await _http.PostAsJsonAsync("/api/transfers", create, ct);
        resp.EnsureSuccessStatusCode();

        var payload = await resp.Content.ReadFromJsonAsync<CreateTransferResult>(cancellationToken: ct)
                     ?? throw new InvalidOperationException("Empty response from /api/transfers");
        _logger.LogInformation("Created transfer {TransferId}", payload.TransferId);
        return payload;
    }

    public async Task<UploadCompleteResult> UploadAsync(Stream file, string transferId, CancellationToken ct = default)
    {
        const int chunkSize = 2 * 1024 * 1024;
        var buffer = new byte[chunkSize];
        int part = 0, read;

        while ((read = await file.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
        {
            part++;
            using var content = new ByteArrayContent(buffer.AsMemory(0, read).ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var resp = await _http.PutAsync($"/api/transfers/{transferId}/parts/{part}", content, ct);
            resp.EnsureSuccessStatusCode();
        }

        var complete = new { ManifestJws = "{ \"jws\": \"<todo>\" }" };
        var r2 = await _http.PostAsJsonAsync($"/api/transfers/{transferId}/complete", complete, ct);
        r2.EnsureSuccessStatusCode();

        return new UploadCompleteResult(transferId, part);
    }
}



