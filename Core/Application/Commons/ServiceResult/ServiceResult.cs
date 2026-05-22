using Core.Application.Interface;
using System.Net;

namespace Core.Application.Commons.ServiceResult;
public class ServiceResult
{
    public bool IsOk { get; }
    public bool IsError => !IsOk;
    public HttpStatusCode Status { get; }
    public List<IServiceError> ErrorMessages { get; }

    public ServiceResult(HttpStatusCode status = HttpStatusCode.OK, bool isOk = true)
    {
        Status = status;
        IsOk = isOk;
        ErrorMessages = new List<IServiceError>();
    }

    public ServiceResult(bool isOk, HttpStatusCode status, string description)
        : this(status, isOk)
    {
        AddError(status, description);
    }

    public void AddError(HttpStatusCode code, string description)
    {
        var error = new ServiceError(code, description);
        ErrorMessages.Add(error);
    }
}
