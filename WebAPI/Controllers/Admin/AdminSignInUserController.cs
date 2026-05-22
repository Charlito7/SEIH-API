using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Route("admin/identity/user")]
    public class AdminSignInUserController : BaseController
    {
        private readonly ISignInUser _service;

        public AdminSignInUserController(ISignInUser service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login", Name ="AdminUserLogin")]
        public async Task<IActionResult> AdminLoginUserAsync([FromBody]UserLoginModel model)
        {
            var result = await _service.SingInAsync(model);

            if(result.IsError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
