using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MSSnackGolFrontend.Controllers
{
    [AllowAnonymous]
    [Route("Error")]
    public class ErrorController : Controller
    {
        [HttpGet("NotFound")]
        public IActionResult NotFoundPage([FromQuery] int? code)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var statusCode = code ?? StatusCodes.Status404NotFound;
            Response.StatusCode = StatusCodes.Status404NotFound;
            ViewData["StatusCode"] = statusCode;
            ViewData["OriginalPath"] = feature?.OriginalPath;
            ViewData["OriginalQuery"] = feature?.OriginalQueryString;
            return View("NotFound");
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("Error");
        }
    }
}
