using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Application.Interface.Services.SEIH;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Hospital;
using WebAPI.Extensions;
using Core.Application.Interface.Security;

namespace WebAPI.Controllers.SEIH.HOSPITALMANAGER;

[Authorize(Roles = "Admin")]
[Route("api/security/keys")]
[ApiController]
public class InstitutionKeyController : ControllerBase
{
    private readonly IInstitutionKeyService _service;

    public InstitutionKeyController(IInstitutionKeyService service)
    {
        _service = service;
    }

    [HttpPost("rotate")]
    public async Task<IActionResult> Rotate(RotateKeyDto dto)
    {
        var result = await _service.RotateKeyAsync(dto);

        if (!result.IsOk)
            return StatusCode((int)result.Status, result.ErrorMessages);

        return StatusCode((int)result.Status, result.Result);
    }
}