using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Application.Interface.Services.SEIH;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Hospital;
using WebAPI.Extensions;
using Core.Domain.Entity.SEIH;

namespace WebAPI.Controllers.SEIH.HOSPITALMANAGER;

// [Route("seih/hospital")]
[Route("api/rest/seih/hospital")]
[ApiController]
[AllowAnonymous]
public class HospitalController : ControllerBase
{
    private readonly IHospitalService _hospitalService;

    public HospitalController(IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("list", Name = "HospitalList")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _hospitalService.GetAllAsync();

        if (result.IsError)
            return BadRequest(result);

        // return Ok(result);
        return Ok(result.Result);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("get-by-id", Name = "HospitalGetById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _hospitalService.GetByIdAsync(id);

        if (result.IsError)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("create", Name = "HospitalCreate")]
    public async Task<IActionResult> Create([FromBody] CreateHospitalRequest request)
    {
        var result = await _hospitalService.CreateWithSecurityAsync(request);
        return result.ToActionResult();
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("update", Name = "Hospitalupdate")]
    public async Task<IActionResult> Update([FromBody] UpdateHospitalRequest request)
    {
        var result = await _hospitalService.UpdateAsync(request);

        if (result.IsError)
            return BadRequest(result);

        return Ok(result.Result);
    }

    // [HttpPost]
    // [AllowAnonymous]
    // [Route("update", Name = "HospitalUpdate")]
    // public async Task<IActionResult> Update([FromBody] HospitalDto dto)
    // {
    //     var result = await _hospitalService.UpdateAsync(dto);
    //     return result.ToActionResult();
    // }

    [HttpPost]
    [AllowAnonymous]
    [Route("delete", Name = "HospitalDelete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var result = await _hospitalService.DeleteAsync(id);
        return result.ToActionResult();
    }
    [HttpPost]
    [AllowAnonymous]
    [Route("activate", Name = "HospitalActivate")]
    public async Task<IActionResult> Activate([FromBody] Guid id)
    {
        var result = await _hospitalService.ActivateAsync(id);
        return result.ToActionResult();
    }
    [HttpPost]
    [AllowAnonymous]
    [Route("deactivate", Name = "HospitalDeactivate")]
    public async Task<IActionResult> Deactivate([FromBody] Guid id)
    {
        var result = await _hospitalService.DeactivateAsync(id);
        return result.ToActionResult();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register-public-key", Name = "HospitalRegisterPublicKey")]
    public async Task<IActionResult> RegisterPublicKey(
        [FromBody] RegisterPublicKeyRequest request)
    {
        var result = await _hospitalService
            .RegisterPublicKeyAsync(User, request.PublicKey);

        return result.ToActionResult();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("get-public-key", Name = "HospitalGetPublicKey")]
    public async Task<IActionResult> GetPublicKey([FromBody] Guid hospitalId)
    {
        var result = await _hospitalService.GetPublicKeyAsync(hospitalId);
        return result.ToActionResult();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("ping", Name = "HospitalPing")]
    public IActionResult Ping()
    {
        Console.WriteLine("PING CONTROLLER REACHED");
        return Ok("SECURE CONNECTION OK");
    }


    [HttpPost("register-transfer-key")]
    public async Task<IActionResult> RegisterTransferKey(
        [FromBody] RegisterTransferKeyRequest request)
    {
        var result = await _hospitalService
            .RegisterTransferKeyAsync(request);

        return result.ToActionResult();
    }
    [HttpPost("network/list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNetworkHospitals()
    {

        Console.WriteLine("NETWORK LIST HIT");
        var hospitals = await _hospitalService.GetNetworkHospitalsAsync();
        return Ok(hospitals);
    }
    [HttpGet("debug/network")]
    [AllowAnonymous]
    public async Task<IActionResult> DebugNetwork()
    {
        var hospitals = await _hospitalService.GetNetworkHospitalsAsync();

        Console.WriteLine("DEBUG NETWORK CALLED");

        return Ok(hospitals);
    }


}

