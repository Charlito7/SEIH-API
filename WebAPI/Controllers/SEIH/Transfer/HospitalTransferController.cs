using Core.Application.Interface.Services.SEIH.Hospital;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;
using Core.Application.Model.Features;
using Core.Domain.Entity.SEIH;
using Core.Application.Model.Features.Transfer;
using SEIHTransfert.Extensions;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Interface.Services.SEIH;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.SEIH.HOSPITALMANAGER;

[ApiController]

[Route("seih/hospital")]
[AllowAnonymous]
public class HospitalTransferController : BaseController
{
    private readonly ITransferService _service;
private readonly IHospitalService _hospitalService;

    public HospitalTransferController(
    ITransferService service,
    IHospitalService hospitalService)
{
    _service = service;
    _hospitalService = hospitalService;
}

    // 🔐 MACHINE TO MACHINE (mTLS)
    [HttpPost("start")]
    [AllowAnonymous]
    public async Task<IActionResult> StartTransfer(
    [FromBody] TransferStartRequestDto request)
    {
        var hospitalId = HttpContext.GetHospitalId();

        var result = await _service.StartAsync(
            hospitalId,
            request);

        if (result.IsError)
            return BadRequest(result);

        return Ok(result);

    }

    // 👤 USER MODE (JWT)
    [HttpPost("get/list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetList()
    {
        var result = await _service.GetTransferListAsync(User);

        if (result.IsError)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("public/list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHospitals()
    {
        var hospitals = await _hospitalService.GetPublicHospitalsAsync();
        return Ok(hospitals);
    }
}

