

using Application.Interfaces.Repositories.User;
using Core.Application.Interfaces.Services.User;
using Infrastructure.Constants;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.User
{
    [Route("user/external/provider")]
    public class ExternalLoginCallBackController : BaseController
    {
        private readonly IMyExternalLoginService _externalLoginService;
        private readonly ISignInManager _signInManger;
        //private readonly IEndPointBuilder _endPointBuilder;

        public ExternalLoginCallBackController(IMyExternalLoginService externalLoginService, ISignInManager signInManager)
        {
            _externalLoginService = externalLoginService;
            _signInManger = signInManager;
          // _endPointBuilder = endPointBuilder;
        }

       /* [AllowAnonymous]
        [HttpGet]
        [Route("auth-callback", Name = "ExternalLoginCallBack")]
        public async Task<IActionResult> ExternalLoginCallBack()
        {
            ExternalLoginInfo info = await _signInManger.GetExternaLLoginInfoAsync();

            if (info == null)
            {
                return NotFound("Provider could not be reached");
            }

            var result = await _externalLoginService.ExternalLoginAsync(info);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            //string path = Path
            //                .Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, EnvFileConstants.TEMP_FILE);

            //if (System.IO.File.Exists(path))
            //{
            //    using (StreamWriter logWriter = System.IO.File.AppendText(path))
            //    {
            //        await logWriter.WriteLineAsync(result.Result.Token);
            //        await logWriter.WriteLineAsync(result.Result.RefreshToken);
            //    }
            //}

            var returnURL = _endPointBuilder.BuildExternalLoginRedirectURL(result.Result.Token!, result.Result.RefreshToken!);
            //var returnURL = $@"http://localhost:5178/?token={result.Result.Token}&refreshToken={result.Result.RefreshToken}";
           
            return Redirect(returnURL.ToString());
        }*/
    }
}
