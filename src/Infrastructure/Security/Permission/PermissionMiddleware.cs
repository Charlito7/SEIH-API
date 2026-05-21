using Core.Application.Interface.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Security.Permission;

public class PermissionMiddleware 
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IUserPermissionService permissionService,
        IPermissionExclusionService exclusionService)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        var method = context.Request.Method.ToUpper();

        // Vérification via le service d’exclusion
        if (exclusionService.IsExcluded(method, path))
        {
            await _next(context);
            return;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var userId = Guid.Parse(userIdClaim.Value);

        var authorized = await permissionService.HasPermissionAsync(userId, method, path);

        if (!authorized)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await _next(context);
    }
}


