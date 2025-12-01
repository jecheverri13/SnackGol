using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Models;
using System.Text.Json;

namespace MSSnackGolFrontend.Controllers
{
    public class QRController : Controller
    {
        private const string CheckoutConfirmationSessionKey = "CheckoutConfirmationPayload";
        private readonly ILogger<QRController> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public QRController(ILogger<QRController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Confirmacion()
        {
            var json = HttpContext.Session.GetString(CheckoutConfirmationSessionKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                TempData["CartFlash"] = "No se encontró un pedido confirmado. Realiza el checkout nuevamente.";
                return RedirectToAction("Index", "Carrito");
            }

            try
            {
                var model = JsonSerializer.Deserialize<CheckoutConfirmationViewModel>(json, _jsonOptions);
                HttpContext.Session.Remove(CheckoutConfirmationSessionKey);
                if (model is null)
                {
                    TempData["CartFlash"] = "No se pudo cargar la confirmación del pedido.";
                    return RedirectToAction("Index", "Carrito");
                }

                if (TempData.TryGetValue("CheckoutFlash", out var flashObj) && flashObj is string flashMessage)
                {
                    ViewBag.CheckoutFlash = flashMessage;
                }

                return View("~/Views/QR/Confirmacion.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo interpretar la confirmación de checkout. Payload: {Payload}", json);
                HttpContext.Session.Remove(CheckoutConfirmationSessionKey);
                TempData["CartFlash"] = "No se pudo cargar la confirmación del pedido.";
                return RedirectToAction("Index", "Carrito");
            }
        }
    }
}
