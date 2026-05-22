using Core.Application.Interface.Security;

namespace Infrastructure.Security.Permission;

public class PermissionExclusionService: IPermissionExclusionService
{
    private static readonly List<(string Method, string PathPrefix)> _excludedRoutes = new()
        {
            ("POST", "/seih/identity/user/loginseih"),
        };

    public bool IsExcluded(string method, string path)
    {
        method = method.ToUpperInvariant();
        path = path.ToLowerInvariant();

        return _excludedRoutes.Any(route =>
            method == route.Method && path.StartsWith(route.PathPrefix));
    }
}
