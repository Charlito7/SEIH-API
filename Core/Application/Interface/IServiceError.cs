using System.Net;

namespace Core.Application.Interface;

public interface IServiceError
{
    HttpStatusCode Code { get; }
    string Description { get; }
}