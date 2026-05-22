namespace Core.Application.Interface.Security;

public interface IUserPermissionService
{
    Task<bool> HasPermissionAsync(Guid userId, string method, string path);
}
