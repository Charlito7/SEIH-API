using Core.Application.Interface;
using System.Net;

namespace Core.Application.Commons.ServiceResult;
public class ServiceError : IServiceError
{
    public HttpStatusCode Code { get; }
    public string Description { get; }

    public ServiceError(HttpStatusCode code, string description)
    {
        Code = code;
        Description = description;
    }
}