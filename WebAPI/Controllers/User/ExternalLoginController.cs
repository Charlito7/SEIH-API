using Application.Interfaces.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.User;
//[ApiController]
[NonController]
[Route("user/external/provider/microsoft")]
public class ExternalLoginController : BaseController
{
    private readonly ISignInManager _signInManager;
    //private readonly IEndPointBuilder _endPointBuilder;

    public ExternalLoginController(ISignInManager signInManager)
    {
        _signInManager = signInManager;
        //_endPointBuilder = endPointBuilder;
    }

    
   /* [AllowAnonymous]
    [HttpGet]
    [Route("login", Name = "ExternalLogin")]
    public IActionResult ExternalLoginAsync()
    {
        var redirectURL = _endPointBuilder.BuildCallBackEndPoint();
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Microsoft", redirectURL); 
        return Challenge(properties, "Microsoft");
    }
}*/
}