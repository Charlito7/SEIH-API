using Core.Application.Interface.Services.SEIH.User;
using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebAPI.Controllers.SEIH.User.SEIHADMIN;

[ApiController]
[Route("seih/manager")]
public class CreateHospitalManagerController : BaseController
{
    private readonly IUserService _service;

    public CreateHospitalManagerController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("createhospitalmanageraccount", Name = "CreateHospitalMANAGERAccount")]
    public async Task<IActionResult> CreateUserAccountAsync(CreateUserModel model)
    {
        var result = await _service.SEIH_CreateUserAsync(User,model);

        if (result.IsError)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
