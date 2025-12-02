using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MSSnackGolFrontend.Models;
using System.Diagnostics;

namespace MSSnackGolFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Página de inicio. Si no está autenticado, redirige al login.
        /// Si está autenticado, muestra la página de inicio.
        /// </summary>
        public IActionResult Index()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
