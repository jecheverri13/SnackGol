using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MSSnackGolFrontend.Views.Carrito
{
    public class ProductoModel : PageModel
    {
        private readonly ILogger<ProductoModel> _logger;

        public ProductoModel(ILogger<ProductoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Producto page accessed");
        }
    }
}