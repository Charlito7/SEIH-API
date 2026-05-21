
using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Model.Features;
using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;
using System.Net;
using System.Security.Claims;

namespace Infrastructure.Services.SEIH.Hospital;

public class HospitalPermissionService : IHospitalPermissionService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHospitalPermissionRepository _hospitalPermissionRepository;
    public HospitalPermissionService(IUsersRepository usersRepository, IMapper mapper, IHospitalPermissionRepository hospitalPermissionRepository)
    {
        _usersRepository = usersRepository;
        _hospitalPermissionRepository = hospitalPermissionRepository;
    }

    public async Task<ServiceResult<IEnumerable<string>>> GetAllPermissionServiceAsync() 
    {
        var permissions = await _hospitalPermissionRepository.GetPermissionAsyncRepository();

        return new ServiceResult<IEnumerable<string>>(permissions);
    }


 
}
