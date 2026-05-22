namespace Core.Application.Interface.Security;

public interface IPermissionExclusionService
{
    bool IsExcluded(string method, string path);
}
