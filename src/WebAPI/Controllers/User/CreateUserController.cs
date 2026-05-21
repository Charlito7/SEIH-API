using Core.Application.Interfaces.Services.User;
using Core.Application.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Route("account/user")]
    public class CreateUserController : BaseController
    {
        private readonly ICreateUserService _service;

        public CreateUserController(ICreateUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create", Name = "CreateUserAccount")]
        public async Task<IActionResult> CreateUserAccountAsync(CreateUserModel model)
        {
            var result = await _service.CreateUserAsync(model);

            if (result.IsError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
