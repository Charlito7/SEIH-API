using Core.Application.Interface.Token;
using Core.Application.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Controllers.Base;
using WebApi.Filters;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Route("user/profile")]
    public class GetUserProfileDataController : AuthorizeBaseController
    {
        private readonly IGetUsersProfileData _service;
        public GetUserProfileDataController(ITokenServices tokenServices, IGetUsersProfileData service) : base(tokenServices)
        {
            _service = service;
        }

        [HttpPost]
        [Route("data/get")]
        [AuthorizeRoles]
        public async Task<IActionResult> GetUsersProfileDataAsync()
        {
            var userEmail = this.User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null || string.IsNullOrEmpty(userEmail))
                return Unauthorized();

            var response = await _service.GetUserProfileDataAsync(userEmail);

            if (response.IsError)
            {
                return NotFound();
            }

            return Ok(response);    
        }
        
    }
}
