using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebAPI.Controllers.User
{
    public class LogoutController : BaseController
    {
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("SessionId", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Logged out" });
        }

    }
}
