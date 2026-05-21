using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH;
using Core.Application.Model.Features;
using Core.Application.Model.Request;
using Core.Domain.Entity.SEIH;
using Core.Domain.Procedures.SEIH;
using Infrastructure.Repository.SEIH.Hospital;
using Infrastructure.Repository.SEIH.User;
using Infrastructure.Security;
using Infrastructure.Utils;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Infrastructure.Services.SEIH.Hospital;

public class HospitalUserService : IHospitalUserService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IHospitalRoleRepository _hospitalRoleRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly IMapper _mapper;
    public HospitalUserService(IUsersRepository usersRepository, IMapper mapper, IHospitalRepository hospitalRepository,
        IHospitalRoleRepository hospitalRoleRepository, IRolesRepository rolesRepository)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
        _hospitalRepository = hospitalRepository;
        _hospitalRoleRepository = hospitalRoleRepository;
        _rolesRepository = rolesRepository;
    }
    public async Task<ServiceResult<bool>> HospitalAddRoleToUserServiceAsync(ClaimsPrincipal claim, AddRolesToUserDTO dataModel)
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
        //Check Role
        var roleCreation = await _hospitalRoleRepository.GetRoleByNameAsync(dataModel.RoleName!, manager.HospitalId);
        if (roleCreation == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }
        //Check User
        var userId = Guid.Parse(dataModel.UserId!);
        var isUserExists = await _usersRepository.GetUserByIdAsync(userId);

        if (isUserExists == null)
        {
            return new ServiceResult<bool>(System.Net.HttpStatusCode.NotAcceptable);
        }

        var IsRoleAssigned = await _rolesRepository.AssignRoles(isUserExists!.Id, roleCreation.Id);

        if (!IsRoleAssigned)
        {
            return new ServiceResult<bool>(System.Net.HttpStatusCode.NotAcceptable);
        }
        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> HospitalCreateUserServiceAsync(ClaimsPrincipal claim, CreateUserModel dataModel)
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
        //User Creation Logic
        var user = _mapper.Map<UsersEntity>(dataModel);
        var isUserExists = await _usersRepository.GetUserByEmailAsync(dataModel.Email!);

        if (isUserExists != null)
        {
            return new ServiceResult<bool>(System.Net.HttpStatusCode.NotAcceptable);
        }

        //Check if the hospital  exists
        var hospital = await _hospitalRepository.GetHospitalByIdAsync(manager.HospitalId);
        if (hospital == null)
        {
            return new ServiceResult<bool>(System.Net.HttpStatusCode.NotAcceptable);
        }

        user.Id = Guid.NewGuid();
        user.Username = hospital.Code!.ToLower() + "-" + UsernameGenerator.CreateUsername(dataModel.FirstName!, dataModel.LastName!);
        user.Salt = PasswordSalt.GenerateSalt();
        user.Email = dataModel.Email!.ToLower();
        dataModel.Password = MyPasswordHasher.HashPassword(user, dataModel.Password!);
        user.PasswordHash = dataModel.Password;
        user.IsNewPasswordRequired = true;
        user.HospitalId = (Guid)hospital.Id!;
        var userCreation = await _usersRepository.CreateUserAsync(user);
        if (!userCreation)
        {
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
        }

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> HospitalUpdateUserPasswordServiceAsync(ClaimsPrincipal claim, ChangePasswordModel dataModel)
    {
        var email = claim.Claims
                      .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
                      .Select(c => c.Value)
                      .FirstOrDefault();

        var user = await _usersRepository.GetUserByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }

        if (!MyPasswordHasher.VerifyHashedPassword(user, user.PasswordHash!,dataModel.OldPassword!))
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }
        
        user.IsNewPasswordRequired = false;
        user.Salt = PasswordSalt.GenerateSalt();
        user.PasswordHash = MyPasswordHasher.HashPassword(user, dataModel.NewPassword!);
        user.LastModified = DateTime.UtcNow;
        user.LastModifiedBy = user.Id.ToString();
        var updateResult = await _usersRepository.UpdateUserAsync(user);

        if (!updateResult)
        {
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
        }

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> HospitalUpdateUserPasswordByManagerServiceAsync(ClaimsPrincipal claim, ChangePasswordByManagerModel dataModel)
    {
        var email = claim.Claims
                      .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
                      .Select(c => c.Value)
                      .FirstOrDefault();

        var user = await _usersRepository.GetUserByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }
        var targetUser = await _usersRepository.GetUserByEmailAsync(dataModel.UserEmail!);
        if (targetUser == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }

        targetUser.IsNewPasswordRequired = true;
        targetUser.Salt = PasswordSalt.GenerateSalt();
        targetUser.PasswordHash = MyPasswordHasher.HashPassword(targetUser, dataModel.NewPassword!);
        targetUser.LastModified = DateTime.UtcNow;
        targetUser.LastModifiedBy = user.Id.ToString();
        var updateResult = await _usersRepository.UpdateUserAsync(targetUser);

        if (!updateResult)
        {
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
        }

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> HospitalUpdateUserServiceAsync(ClaimsPrincipal claim, CreateUserModel dataModel)
    {
        var email = claim.Claims
              .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
              .Select(c => c.Value)
              .FirstOrDefault();

        var user = await _usersRepository.GetUserByEmailAsync(email!);
        if (user == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }
        var targetUser = await _usersRepository.GetUserByEmailAsync(dataModel.Email!);
        if (targetUser == null)
        {
            return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
        }

        targetUser.FirstName = dataModel.FirstName;
        targetUser.LastName = dataModel.LastName;
        targetUser.Email = dataModel.Email!.ToLower();
        targetUser.LastModified = DateTime.UtcNow;
        targetUser.LastModifiedBy = user.Id.ToString();
        var updateResult = await _usersRepository.UpdateUserAsync(targetUser);

        if (!updateResult)
        {
            return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
        }

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<IEnumerable<GetUserListWithRolesResponse>>> HospitalGetUsersListWithRolesAsync(ClaimsPrincipal claim)
    {
        var email = claim.Claims
               .Where(c => c.Type == System.Security.Claims.ClaimTypes.Email)
               .Select(c => c.Value)
               .FirstOrDefault();

        var manager = await _usersRepository.GetUserByEmailAsync(email!);
        if (manager == null)
        {
            return new ServiceResult<IEnumerable<GetUserListWithRolesResponse>>(HttpStatusCode.Unauthorized);
        }

        var users = await _usersRepository.GetAllHospitalUsersWithRolesAsync(manager.HospitalId!);

        return new ServiceResult<IEnumerable<GetUserListWithRolesResponse>>(users);
    }
}
