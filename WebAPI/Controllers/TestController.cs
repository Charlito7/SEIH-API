using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/")]
public class TestController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            status = "ok",
            message = "SEIH API is running",
            timestamp = DateTime.UtcNow,
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        });
    }
}