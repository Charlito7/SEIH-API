using Microsoft.AspNetCore.Mvc;
using Core.Application.Commons.ServiceResult;

namespace WebAPI.Extensions
{
    public static class ServiceResultExtensions
    {
        public static IActionResult ToActionResult<T>(
            this ServiceResult<T> result)
        {
            if (result == null)
                return new StatusCodeResult(500);

            if (result.IsOk)
            {
                return new ObjectResult(result.Result)
                {
                    StatusCode = (int)result.Status
                };
            }

            return new ObjectResult(result.ErrorMessages)
            {
                StatusCode = (int)result.Status
            };
        }

        public static IActionResult ToActionResult(
            this ServiceResult result)
        {
            if (result == null)
                return new StatusCodeResult(500);

            if (result.IsOk)
            {
                return new StatusCodeResult((int)result.Status);
            }

            return new ObjectResult(result.ErrorMessages)
            {
                StatusCode = (int)result.Status
            };
        }
    }
}
