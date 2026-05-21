using Core.Domain.Entity;
using Core.Domain.Entity.SEIH;

namespace Core.Application.Interface.Repository.SEIH.Hospital;

public interface IHospitalPermissionRepository
{
    Task<IEnumerable<string>> GetPermissionAsyncRepository();
}
