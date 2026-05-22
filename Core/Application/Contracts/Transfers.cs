using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts;

public class Transfers
{
    public record CreateTransferResult(string TransferId, string UploadToken);
    public record UploadCompleteResult(string TransferId, int PartsUploaded);
}
