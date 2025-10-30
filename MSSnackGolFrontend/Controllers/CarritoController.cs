using Microsoft.AspNetCore.Mvc;

namespace MSSnackGolFrontend.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}