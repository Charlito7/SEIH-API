using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Core.Application.Contracts.Transfers;

namespace Application.Abstractions;

public interface ISeihTransferClient
{
    Task<CreateTransferResult> PrepareAsync(long fileSize, string recipientHospitalId, CancellationToken ct = default);
    Task<UploadCompleteResult> UploadAsync(Stream file, string transferId, CancellationToken ct = default);
}