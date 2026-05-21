
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security.Permission;

public class UserPermissionService : IUserPermissionService
{
    private readonly AppDbContext _context;
    private readonly IUsersRepository _usersRepository;

    public UserPermissionService(AppDbContext context, IUsersRepository usersRepository)
    {
        _context = context;
        _usersRepository = usersRepository;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string method, string path) 
    {
        var normalizedPath = NormalizePath(path);
        var target = $"{method}_{normalizedPath}";

       

        // Get role entities for the user
        var roles = await _usersRepository.GetUserRolesWithPermissionAsync(userId);


        var permissions = roles
       .Select(r => $"{r.HttpMethod}_/{NormalizePath(r.Path!)}")
       .Distinct();

        var isOk = permissions.Contains(target);
        return isOk;
    }

    private string? NormalizePath(string path)
    {
        // Ex: /api/patients/123 => /api/patients/{id}
        // (Tu peux faire mieux avec des regex selon ta convention de routage)
        if(path == null) {
            return null;
        }
        var segments = path.Split('/');
        for (int i = 0; i < segments.Length; i++)
        {
            if (Guid.TryParse(segments[i], out _) || int.TryParse(segments[i], out _))
                segments[i] = "{id}";
        }

        return string.Join('/', segments);
    }
}
