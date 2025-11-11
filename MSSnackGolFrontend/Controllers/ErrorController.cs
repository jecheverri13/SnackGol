using Microsoft.AspNetCore.Mvc;

namespace MSSnackGolFrontend.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [HttpGet("NotFound")]
        public IActionResult NotFoundPage([FromQuery] int? code)
        {
            // Ensure correct status code
            Response.StatusCode = 404;
            // Pass the original status code optionally for diagnostics
            ViewData["StatusCode"] = code ?? 404;
            return View("NotFound");
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            Response.StatusCode = 500;
            return View("Error");
        }
    }
}
