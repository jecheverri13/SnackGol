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

        /// <summary>
        /// Muestra la página de confirmación de pago
        /// </summary>
        /// <param name="subtotal">Subtotal a pagar</param>
        /// <returns>Vista de confirmación de pago</returns>
        public IActionResult ConfirmarPago(decimal subtotal = 0)
        {
            var modelo = new PagoViewModel
            {
                Fecha = DateTime.Now,
                Subtotal = subtotal
            };

            return View(modelo);
        }

        /// <summary>
        /// Procesa el pago enviado desde el formulario
        /// </summary>
        /// <param name="modelo">Datos del pago</param>
        /// <returns>Redirige a confirmación o vuelve al formulario con errores</returns>
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
                // Llamar a API backend para validar y procesar el pago
                var client = _httpClientFactory.CreateClient();
                var apiUrl = "http://localhost:5046/api/pago/procesar"; // Puerto correcto del backend

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

                        // Extraer el código de confirmación
                        string codigoConfirmacion = "PAGO-SIN-CODIGO";
                        if (root.TryGetProperty("codigoConfirmacion", out var codElement))
                        {
                            codigoConfirmacion = codElement.GetString() ?? "PAGO-SIN-CODIGO";
                        }
                        else if (root.TryGetProperty("codigo", out var codElement2))
                        {
                            codigoConfirmacion = codElement2.GetString() ?? "PAGO-SIN-CODIGO";
                        }

                        // Obtener el token de sesión del carrito
                        var sessionToken = HttpContext.Session.GetString("SessionToken") ?? "guest-" + Guid.NewGuid().ToString().Substring(0, 8);

                        // Crear la orden en el backend usando OrderManagement/Create
                        await CreateOrderInBackend(client, sessionToken, modelo.Total, codigoConfirmacion);

                        // IMPORTANTE: Limpiar la sesión ANTES de crear el nuevo CheckoutConfirmationPayload
                        HttpContext.Session.Remove("CheckoutConfirmationPayload");

                        // Crear modelo para la vista QR
                        var confirmacionViewModel = new CheckoutConfirmationViewModel
                        {
                            OrderId = codigoConfirmacion,
                            Status = "Confirmed",
                            Total = (double)modelo.Total,
                            Pickup = new CheckoutConfirmationViewModel.PickupInfo
                            {
                                Code = codigoConfirmacion,
                                GeneratedAtUtc = DateTime.UtcNow,
                                // Generar un payload JSON simple que la vista puede convertir a QR
                                PayloadBase64 = GeneratePayloadBase64(codigoConfirmacion, (double)modelo.Total),
                                QrImageBase64 = null  // Se generará en la vista desde el payload
                            }
                        };

                        // Guardar en sesión con la clave esperada por QRController
                        HttpContext.Session.SetString("CheckoutConfirmationPayload", JsonSerializer.Serialize(confirmacionViewModel));
                        TempData["SuccessMessage"] = "Pago procesado exitosamente.";
                        TempData["CodigoConfirmacion"] = codigoConfirmacion;

                        // Redirigir a la vista QR
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

        /// <summary>
        /// Crea una orden en el backend después de procesar el pago
        /// </summary>
        private async Task CreateOrderInBackend(HttpClient client, string sessionToken, decimal total, string codigoConfirmacion)
        {
            try
            {
                // Primero, asegurar que el cliente existe en la BD
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
                            description = "1.0", // Debe ser un número válido (>= 0.01)
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
                // No lanzar excepción - el pago ya se procesó, registrar en logs
            }
        }

        /// <summary>
        /// Asegura que el cliente existe en la BD, lo crea si es necesario
        /// </summary>
        private async Task EnsureClientExists(HttpClient client, string sessionToken)
        {
            try
            {
                // Intentar crear el cliente (si ya existe, el endpoint maneja gracefully)
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
                // No lanzar excepción - continuar con el flujo
            }
        }

        /// <summary>
        /// Genera un payload JSON codificado en Base64 para el QR
        /// </summary>
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

        /// <summary>
        /// Muestra página de pago exitoso (legacy - se usa QR/Confirmacion en su lugar)
        /// </summary>
        public IActionResult PagoExitoso()
        {
            var codigoConfirmacion = TempData["CodigoConfirmacion"]?.ToString() ?? "N/A";
            ViewBag.CodigoConfirmacion = codigoConfirmacion;
            return View();
        }

        /// <summary>
        /// Muestra página de error en pago
        /// </summary>
        public IActionResult PagoFallido()
        {
            return View();
        }
    }
}
