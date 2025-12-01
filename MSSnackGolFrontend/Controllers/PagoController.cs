using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Models;
using System.Text.Json;
using LibraryConnection.Dtos;
using LibraryEntities.Models;

namespace MSSnackGolFrontend.Controllers
{
    public class PagoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PagoController> _logger;

        public PagoController(IHttpClientFactory httpClientFactory, ILogger<PagoController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /*
        <summary>
        Muestra la página de confirmación de pago
        </summary>
        <param name="subtotal">Subtotal a pagar</param>
        <returns>Vista de confirmación de pago</returns> */
        public IActionResult ConfirmarPago(decimal subtotal = 0)
        {
            var modelo = new PagoViewModel
            {
                Fecha = DateTime.Now,
                Subtotal = subtotal
            };

            return View(modelo);
        }
        /*
          <summary>
          Procesa el pago enviado desde el formulario.
          
          Flujo:
          1. Valida los datos del formulario (método de pago, número de cuenta/tarjeta)
          2. Envía los datos al backend para validar el pago (/api/pago/procesar)
          3. Obtiene el código de confirmación de la respuesta
          4. Crea un cliente en la BD si no existe
          5. Registra la orden en el backend
          6. Genera un payload Base64 para el QR
          7. Redirige a la vista de confirmación con QR
          
          Ejemplo de uso:
          POST /Pago/ProcesarPago
          {
            "metodoPago": "nequi",
            "numeroCuentaNequi": "3001234567",
            "subtotal": 9200,
            "total": 10948
          }
          Respuesta: Redirige a QR/Confirmacion con código PAGO-20251201120000-xxxx
          </summary>
          <param name="modelo">Datos del pago incluyendo método, monto y número de cuenta/tarjeta</param>
          <returns>Redirige a QR/Confirmacion si es exitoso, o regresa al formulario si hay error</returns> */
        [HttpPost]
        public async Task<IActionResult> ProcesarPago(PagoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Verifique que todos los campos sean válidos.";
                return RedirectToAction("ConfirmarPago", new { subtotal = modelo.Subtotal });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiUrl = "http://localhost:5046/api/pago/procesar";

                var content = new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        metodoPago = modelo.MetodoPago,
                        numeroCuentaNequi = modelo.NumeroCuentaNequi,
                        numeroTarjeta = modelo.NumeroTarjeta,
                        subtotal = modelo.Subtotal,
                        iva = modelo.IVA,
                        total = modelo.Total
                    }),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(responseContent);
                        var root = jsonDoc.RootElement;

                        string codigoConfirmacion = "PAGO-SIN-CODIGO";
                        if (root.TryGetProperty("codigoConfirmacion", out var codElement))
                        {
                            codigoConfirmacion = codElement.GetString() ?? "PAGO-SIN-CODIGO";
                        }
                        else if (root.TryGetProperty("codigo", out var codElement2))
                        {
                            codigoConfirmacion = codElement2.GetString() ?? "PAGO-SIN-CODIGO";
                        }

                        var sessionToken = HttpContext.Session.GetString("SessionToken") ?? "guest-" + Guid.NewGuid().ToString().Substring(0, 8);

                        await CreateOrderInBackend(client, sessionToken, modelo.Total, codigoConfirmacion);

                        HttpContext.Session.Remove("CheckoutConfirmationPayload");

                        var confirmacionViewModel = new CheckoutConfirmationViewModel
                        {
                            OrderId = codigoConfirmacion,
                            Status = "Confirmed",
                            Total = (double)modelo.Total,
                            Pickup = new CheckoutConfirmationViewModel.PickupInfo
                            {
                                Code = codigoConfirmacion,
                                GeneratedAtUtc = DateTime.UtcNow,
                                PayloadBase64 = GeneratePayloadBase64(codigoConfirmacion, (double)modelo.Total),
                                QrImageBase64 = null
                            }
                        };

                        HttpContext.Session.SetString("CheckoutConfirmationPayload", JsonSerializer.Serialize(confirmacionViewModel));
                        TempData["SuccessMessage"] = "Pago procesado exitosamente.";
                        TempData["CodigoConfirmacion"] = codigoConfirmacion;

                        return RedirectToAction("Confirmacion", "QR");
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError($"Error al parsear respuesta del API: {ex.Message}");
                        TempData["ErrorMessage"] = "Error al procesar la respuesta del servidor.";
                        return RedirectToAction("ConfirmarPago", new { subtotal = modelo.Subtotal });
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error al procesar pago (Status {response.StatusCode}): {errorContent}");
                    TempData["ErrorMessage"] = $"Error al procesar el pago: {response.StatusCode}. Intente nuevamente.";
                    return RedirectToAction("ConfirmarPago", new { subtotal = modelo.Subtotal });
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error de conexión con el API: {ex.Message}");
                TempData["ErrorMessage"] = "No se pudo conectar con el servidor. Por favor intente más tarde.";
                return RedirectToAction("ConfirmarPago", new { subtotal = modelo.Subtotal });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Excepción al procesar pago: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("ConfirmarPago", new { subtotal = modelo.Subtotal });
            }
        }
        /*
          <summary>
          Crea una orden en el backend después de procesar el pago.
          
          Pasos:
          1. Verifica/crea el cliente con el sessionToken
          2. Construye la solicitud de orden con los detalles del pago
          3. POST a /api/OrderManagement/Create
          4. Registra el resultado en los logs
          
          Ejemplo:
          CreateOrderInBackend(client, "guest-abc123", 10948m, "PAGO-20251201120000-xxxx")
          Crea una orden con customer_id="guest-abc123", total=10948, voucher_number=código de confirmación
          </summary>
          <param name="client">Cliente HTTP configurado</param>
          <param name="sessionToken">Token de sesión o ID del cliente</param>
          <param name="total">Monto total del pedido</param>
          <param name="codigoConfirmacion">Código de confirmación del pago</param>*/
        private async Task CreateOrderInBackend(HttpClient client, string sessionToken, decimal total, string codigoConfirmacion)
        {
            try
            {
                await EnsureClientExists(client, sessionToken);

                var orderRequest = new OrderRequest
                {
                    customer_id = sessionToken,
                    order_date = DateTime.UtcNow,
                    total_gross_amount = (double)total,
                    total_net_price = (double)total,
                    payment_system = "Nequi/TarjetaCredito",
                    voucher_number = codigoConfirmacion,
                    status_fe = "Confirmed",
                    order_lines = new List<OrderLineRequest>
                    {
                        new OrderLineRequest
                        {
                            item = "PAGO",
                            description = "1.0",
                            grossAmount = (double)total,
                            netPrice = (double)total,
                            taxAmount = 0,
                            quantity = 1
                        }
                    }
                };

                var orderContent = new StringContent(
                    JsonSerializer.Serialize(orderRequest),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var orderResponse = await client.PostAsync("http://localhost:5046/api/OrderManagement/Create", orderContent);
                
                if (!orderResponse.IsSuccessStatusCode)
                {
                    var errorContent = await orderResponse.Content.ReadAsStringAsync();
                    _logger.LogWarning($"No se registró la orden en el backend (Status {orderResponse.StatusCode}): {errorContent}");
                }
                else
                {
                    _logger.LogInformation($"Orden creada exitosamente en el backend con código: {codigoConfirmacion}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al registrar la orden en el backend: {ex.Message}");
            }
        }
        /*
          <summary>
          Asegura que el cliente existe en la base de datos.
          Si el cliente no existe, lo crea automáticamente.
          
          Comportamiento:
          - Construye un objeto ClientRequest con sessionToken como document
          - POST a /api/ClientManagement/Create
          - Si el cliente ya existe, el endpoint lo maneja gracefully
          - Si falla, registra en logs pero no interrumpe el flujo
          
          Ejemplo:
          EnsureClientExists(client, "guest-abc123")
          Crea/verifica cliente con document="guest-abc123", name="Cliente Invitado SnackGol"
          </summary>
          <param name="client">Cliente HTTP configurado</param>
          <param name="sessionToken">Token de sesión que será usado como document del cliente</param>
        */
        private async Task EnsureClientExists(HttpClient client, string sessionToken)
        {
            try
            {
                var clientRequest = new
                {
                    document = sessionToken,
                    name = "Cliente Invitado SnackGol",
                    emailAddress = "",
                    docType = "SESSION"
                };

                var clientContent = new StringContent(
                    JsonSerializer.Serialize(clientRequest),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var clientResponse = await client.PostAsync("http://localhost:5046/api/ClientManagement/Create", clientContent);
                
                if (!clientResponse.IsSuccessStatusCode)
                {
                    var errorContent = await clientResponse.Content.ReadAsStringAsync();
                    _logger.LogWarning($"No se pudo crear/verificar cliente (Status {clientResponse.StatusCode}): {errorContent}");
                }
                else
                {
                    _logger.LogInformation($"Cliente asegurado en el backend: {sessionToken}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear/verificar cliente en el backend: {ex.Message}");
            }
        }
        /*
          <summary>
          Genera un payload JSON codificado en Base64 para la generación del QR.
          
          Estructura del payload:
          {
            "OrderId": "PAGO-20251201120000-xxxx",
            "PickupCode": "PAGO-20251201120000-xxxx",
            "Amount": 10948.0,
            "Currency": "COP",
            "GeneratedAt": "2025-12-01T12:00:00.0000000Z",
            "Type": "SnackGolPickup"
          }
          
          Ejemplo:
          var payload = GeneratePayloadBase64("PAGO-20251201120000-1234", 10948.0)
          Retorna: "eyJPcmRlcklkIjoiUEFHTy0yMDI1MTIwMTEyMDAwMC0xMjM0IiwgIlBpY2t1cENvZGUiOi4uLiJ9"
          Este payload se decodifica y transforma en QR en la vista
          </summary>
          <param name="codigoConfirmacion">Código de confirmación del pago</param>
          <param name="total">Monto total a pagar</param>
          <returns>Payload JSON codificado en Base64, o string vacío si hay error</returns>
        */
        private string GeneratePayloadBase64(string codigoConfirmacion, double total)
        {
            try
            {
                var payload = new
                {
                    OrderId = codigoConfirmacion,
                    PickupCode = codigoConfirmacion,
                    Amount = total,
                    Currency = "COP",
                    GeneratedAt = DateTime.UtcNow.ToString("O"),
                    Type = "SnackGolPickup"
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var payloadBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
                return Convert.ToBase64String(payloadBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al generar payload Base64: {ex.Message}");
                return string.Empty;
            }
        }
        /*
          <summary>
          Muestra página de pago exitoso (legacy - se usa QR/Confirmacion en su lugar)
          </summary>
          */
        public IActionResult PagoExitoso()
        {
            var codigoConfirmacion = TempData["CodigoConfirmacion"]?.ToString() ?? "N/A";
            ViewBag.CodigoConfirmacion = codigoConfirmacion;
            return View();
        }
        /*
          <summary>
          Muestra página de error en pago
          </summary>
          */
        public IActionResult PagoFallido()
        {
            return View();
        }
    }
}
