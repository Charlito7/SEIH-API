using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers.SEIH.HOSPITALMANAGER;

using Microsoft.AspNetCore.Mvc;
using Core.Application.Model.Features;

[ApiController]
[Route("dme/network/transfer-request")]
[AllowAnonymous]
public class TransferRequestNetworkController : ControllerBase
{
    private readonly ITransferRequestNetworkService _service;

    public TransferRequestNetworkController(
        ITransferRequestNetworkService service)
    {
        _service = service;
    }

    [HttpPost("receive")]
    [AllowAnonymous]
    public async Task<IActionResult> Receive( [FromBody] TransferRequestNetworkDto dto)
    {
        var result = await _service.ReceiveAsync(dto);

        if (!result)
            return BadRequest();

        return Ok();
    }

    [HttpPost("incoming")]
    public async Task<IActionResult> Incoming([FromBody] GetIncomingRequestsDto dto)
    {
        if (dto == null)
            return BadRequest("Body is missing");

        var result = await _service.GetIncomingAsync(dto.HospitalId);

        return Ok(result);
    }

    [HttpPost("response")]
[AllowAnonymous]
public async Task<IActionResult> ReceiveResponse(
    [FromBody] TransferRequestResponseNetworkDto dto)
{
    var result = await _service.UpdateStatusAsync(dto);

    if (!result)
        return BadRequest();

    return Ok();
}
}