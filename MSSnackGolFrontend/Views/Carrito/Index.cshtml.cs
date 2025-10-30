using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MSSnackGolFrontend.Views.Carrito
{
    public class CarritoModel : PageModel
    {
        private readonly ILogger<CarritoModel> _logger;

        public CarritoModel(ILogger<CarritoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Carrito page accessed");
        }
    }
}