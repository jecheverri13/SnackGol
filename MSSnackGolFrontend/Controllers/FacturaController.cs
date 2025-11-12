using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Models;
using System.Text.Json;

namespace MSSnackGolFrontend.Controllers
{
    public class FacturaController : Controller
    {
        public IActionResult Index()
        {
            if (TempData.ContainsKey("CartJson"))
            {
                var json = TempData["CartJson"] as string;
                try
                {
                    var cart = JsonSerializer.Deserialize<Models.CartPageViewModel>(json ?? "");
                    var factura = new FacturaViewModel();

                    if (cart?.Cart?.items != null)
                    {
                        foreach (var ci in cart.Cart.items)
                        {
                            factura.Items.Add(new FacturaItem
                            {
                                ProductoId = ci.product_id,
                                Nombre = ci.name,
                                Cantidad = ci.quantity,
                                PrecioUnitario = Convert.ToDecimal(ci.unit_price)
                            });
                        }
                    }

                    factura.Impuesto = Math.Round(factura.Subtotal * 0.12m, 2); // IVA ejemplo 12%
                    // intentar obtener cliente desde cart si existe (no obligatorio)
                    factura.ClienteNombre = string.Empty;
                    factura.Email = string.Empty;

                    return View(factura);
                }
                catch
                {
                    // ignorar y mostrar factura vacía
                }
            }

            return View(new FacturaViewModel());
        }
    }
}
