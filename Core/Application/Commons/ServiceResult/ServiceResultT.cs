using Core.Application.Interface;
using System.Net;

namespace Core.Application.Commons.ServiceResult;

public class ServiceResult<T>:ServiceResult
{
    public T Result { get; }
    public bool IsOk { get; }
    public HttpStatusCode Status { get; }
    public List<IServiceError> ErrorMessages { get; }

    public ServiceResult(HttpStatusCode status) : base(status, false)
    {

    }


#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public ServiceResult(T result, bool isOk = true, HttpStatusCode status = HttpStatusCode.OK, string description = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    {
        Result = result;
        IsOk = isOk;
        Status = status;
        ErrorMessages = new List<IServiceError>();

        if (!isOk)
        {
            AddError(status, description ?? "An error occurred.");
        }
    }

    public void AddError(HttpStatusCode code, string description)
    {
        var error = new ServiceError(code, description);
        ErrorMessages.Add(error);
    }
}