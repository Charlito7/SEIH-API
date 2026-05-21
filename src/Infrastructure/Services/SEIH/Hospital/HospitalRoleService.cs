
using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Model.Features;
using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;
using Infrastructure.Repository.SEIH.Hospital;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Infrastructure.Services.SEIH.Hospital;

public class HospitalRoleService : IHospitalRoleService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHospitalRoleRepository _hospitalRoleRepository;
    public HospitalRoleService(IUsersRepository usersRepository, IMapper mapper, IHospitalRoleRepository hospitalRoleRepository)
    {
        _usersRepository = usersRepository;
        _hospitalRoleRepository = hospitalRoleRepository;
    }

    public async Task<ServiceResult<IEnumerable<string>>> GetAllRoleServiceAsync(ClaimsPrincipal claim)
    {
        var email = claim.Claims
      .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
      .Select(c => c.Value)
      .FirstOrDefault();

        var manager = await _usersRepository.GetUserByEmailAsync(email!);
        if (manager == null)
        {
            //return new ServiceResult<string>(HttpStatusCode.Unauthorized);
        }

        var roles = await _hospitalRoleRepository.GetRoleListAsyncRepository(manager.HospitalId.ToString());

        return new ServiceResult<IEnumerable<string>>(roles);
        
    }

    public async Task<ServiceResult<bool>> HospitalAddPermissionToRoleServiceAsync(ClaimsPrincipal claim, HospitalRolePermissionDto dataModel)
    {
        var email = claim.Claims
              .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
              .Select(c => c.Value)
              .FirstOrDefault();

        var manager = await _usersRepository.GetUserByEmailAsync(email!);
        if (manager == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }

        foreach (var permissionName in dataModel.PermissionName!)
        {
            // Check if the permission exists
            var permission = await _hospitalRoleRepository.GetPermissionByNameAsync(permissionName);
            if (permission == null)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }
            var role = await _hospitalRoleRepository.GetRoleByNameAsync(dataModel.RoleName!, manager.HospitalId);
            if (role == null)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }
                // Create the role permission entity
                RolePermissionEntity rolePermission = new RolePermissionEntity
            {
                Id = Guid.NewGuid(),
                RoleId = (Guid)role.Id!,
                PermissionId = (Guid)permission.Id!,
                CreatedBy = manager.Id.ToString(),
                Created = DateTime.UtcNow,
                IsDeleted = false,
            };

            // Add the permission to the role    
            var permissionAddition = await _hospitalRoleRepository.HospitalAddPermissionToRoleUserAsync(rolePermission);
            if (!permissionAddition)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            }
        }
        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> HospitalCreateRoleServiceAsync(ClaimsPrincipal claim, HospitalRoleDto dataModel)
    {
        var email = claim.Claims
                 .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
                 .Select(c => c.Value)
                 .FirstOrDefault();

        var manager = await _usersRepository.GetUserByEmailAsync(email!);
        if (manager == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }

        RolesEntity role = new RolesEntity
        {
            Id = Guid.NewGuid(),
            Name = dataModel.RoleName,
            HospitalId = manager.HospitalId,
            IsBasicRole = false,
            CreatedBy = manager.Id.ToString(),
            Created = DateTime.UtcNow,
            IsDeleted = false,
        };
       


        var roleCreation = await _hospitalRoleRepository.HospitalCreateRoleAsync(role);
        if (!roleCreation)
        { 
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
        }

        return new ServiceResult<bool>(true);
    }
}
