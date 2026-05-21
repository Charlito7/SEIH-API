using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.User;
using Core.Application.Model.Features;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebAPI.Controllers.SEIH.HOSPITALMANAGER;

[ApiController]
[Route("seih/hospital/permission")]
public class HospitalPermissionController : BaseController
{
    private readonly IHospitalPermissionService _service;

    public HospitalPermissionController(IHospitalPermissionService service)
    {
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("get/list", Name = "PermissionList")]
    public async Task<IActionResult> GetPermissionListAsync()
    {
        var result = await _service.GetAllPermissionServiceAsync();

        if (result.IsError)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

}
