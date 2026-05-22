using AutoMapper;
using Core.Application.Commons.ServiceResult;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Services.SEIH.User;
using Core.Application.Model.Features;
using Core.Application.Model.Request;
using Core.Domain.Entities;
using Core.Domain.Entity.SEIH;
using Infrastructure.Security;
using Infrastructure.Utils;
using System.Net;
using System.Security.Claims;

namespace Infrastructure.Services.SEIH.User
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            IMapper mapper,
            AppDbContext applicationDbContext,
            IHospitalRepository hospitalRepository)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _mapper = mapper;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<ServiceResult<bool>> SEIH_AddRolesToUserAsync(ClaimsPrincipal claim, AddRolesToUserDto dataModel)
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
            var user = await _usersRepository.GetUserByEmailAsync(dataModel.UserEmail!);
            if (user == null)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }

            //Check if the role exists in the hospital
            var hospitalRole = await _rolesRepository.GetHospitalRole(manager.HospitalId, dataModel.RoleName!);
            if (hospitalRole == null)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }
            //Check if the user already has the role
            var existingRoles = await _rolesRepository.GetUserRole((Guid)user.Id!, (Guid)hospitalRole.Id!);
            if(existingRoles != null)
            {
                return new ServiceResult<bool>(HttpStatusCode.NotAcceptable);
            }
            //Assign the role to the user
            var assignRole = await _rolesRepository.AssignRoles(user.Id, hospitalRole.Id);

            if (!assignRole)
            {
                return new ServiceResult<bool>(HttpStatusCode.InternalServerError);
            }
            return new ServiceResult<bool>(true);
        }

        public async Task<ServiceResult<bool>> SEIH_CreateUserAsync(ClaimsPrincipal claim, CreateUserModel dataModel)
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
            var hospital = await _hospitalRepository.GetHospitalByNameAsync(dataModel.HospitalName!);
            if(hospital == null)
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
    }
}
