using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Infrastructure;
using MSSnackGolFrontend.Models;
using System.Text.Json;

namespace MSSnackGolFrontend.Controllers
{
    /// <summary>
    /// Controlador para gestionar la visualización de pedidos del usuario.
    /// </summary>
    public class PedidosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PedidosController> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public PedidosController(IHttpClientFactory httpClientFactory, ILogger<PedidosController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Muestra la lista de pedidos del usuario actual (basado en su token de sesión).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessionToken = SessionTokenHelper.GetOrCreate(HttpContext);
            
            try
            {
                var client = _httpClientFactory.CreateClient("Api");
                client.DefaultRequestHeaders.Add("X-Session-Token", sessionToken);

                var response = await client.GetAsync($"api/OrderManagement/Session/{sessionToken}");
                
                var model = new PedidosViewModel
                {
                    Orders = new List<OrderSummaryViewModel>()
                };

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogDebug("API Response JSON: {Json}", json);
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<OrderSummaryDto>>>(json, _jsonOptions);
                    
                    if (apiResponse?.response != null)
                    {
                        model.Orders = apiResponse.response.Select(o => new OrderSummaryViewModel
                        {
                            OrderId = o.orderId ?? "",
                            Status = o.status ?? "Confirmed",
                            StatusDisplay = GetStatusDisplay(o.status),
                            StatusClass = GetStatusClass(o.status),
                            Total = o.total,
                            OrderDate = o.orderDate,
                            PickupCode = o.pickupCode,
                            ItemCount = o.itemCount,
                            Items = o.items?.Select(i => new OrderItemViewModel
                            {
                                ProductName = i.productName ?? "Producto",
                                Quantity = i.quantity,
                                UnitPrice = i.unitPrice,
                                Subtotal = i.subtotal
                            }).ToList() ?? new List<OrderItemViewModel>()
                        }).OrderByDescending(o => o.OrderDate).ToList();
                    }
                }
                else if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Error al obtener pedidos. StatusCode: {StatusCode}", response.StatusCode);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los pedidos");
                TempData["ErrorFlash"] = "No se pudieron cargar tus pedidos. Intenta de nuevo.";
                return View(new PedidosViewModel { Orders = new List<OrderSummaryViewModel>() });
            }
        }

        /// <summary>
        /// Muestra el detalle de un pedido específico.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Detalle(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction(nameof(Index));
            }

            var sessionToken = SessionTokenHelper.GetOrCreate(HttpContext);

            try
            {
                var client = _httpClientFactory.CreateClient("Api");
                client.DefaultRequestHeaders.Add("X-Session-Token", sessionToken);

                var response = await client.GetAsync($"api/OrderManagement/{id}/pickup");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorFlash"] = "No se encontró el pedido solicitado.";
                    return RedirectToAction(nameof(Index));
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<OrderDetailDto>>(json, _jsonOptions);

                if (apiResponse?.response == null)
                {
                    TempData["ErrorFlash"] = "No se pudo cargar el detalle del pedido.";
                    return RedirectToAction(nameof(Index));
                }

                var detail = apiResponse.response;
                var model = new CheckoutConfirmationViewModel
                {
                    OrderId = detail.orderId,
                    Status = detail.status ?? "Confirmed",
                    Total = detail.total,
                    Pickup = new CheckoutConfirmationViewModel.PickupInfo
                    {
                        Code = detail.pickupCode,
                        QrImageBase64 = detail.pickupQrImageBase64,
                        PayloadBase64 = detail.pickupPayloadBase64,
                        GeneratedAtUtc = detail.generatedAtUtc
                    }
                };

                return View("~/Views/QR/Confirmacion.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el detalle del pedido {OrderId}", id);
                TempData["ErrorFlash"] = "Error al cargar el pedido.";
                return RedirectToAction(nameof(Index));
            }
        }

        private static string GetStatusDisplay(string? status) => status?.ToLowerInvariant() switch
        {
            "confirmed" => "Confirmado",
            "preparing" => "En preparación",
            "readyforpickup" => "Listo para recoger",
            "delivered" => "Entregado",
            _ => "Confirmado"
        };

        private static string GetStatusClass(string? status) => status?.ToLowerInvariant() switch
        {
            "confirmed" => "status-confirmed",
            "preparing" => "status-preparing",
            "readyforpickup" => "status-ready",
            "delivered" => "status-delivered",
            _ => "status-confirmed"
        };
    }

    #region DTOs para deserializar respuestas de la API
    
    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public int status { get; set; }
        public T? response { get; set; }
        public string? errors { get; set; }
    }

    public class OrderSummaryDto
    {
        public string? orderId { get; set; }
        public string? status { get; set; }
        public double total { get; set; }
        public DateTime orderDate { get; set; }
        public string? pickupCode { get; set; }
        public int itemCount { get; set; }
        public List<OrderItemDto>? items { get; set; }
    }

    public class OrderDetailDto
    {
        public string? orderId { get; set; }
        public string? status { get; set; }
        public double total { get; set; }
        public string? pickupCode { get; set; }
        public string? pickupQrImageBase64 { get; set; }
        public string? pickupPayloadBase64 { get; set; }
        public DateTime? generatedAtUtc { get; set; }
        public DateTime? deliveredAtUtc { get; set; }
        public List<OrderItemDto>? items { get; set; }
    }

    public class OrderItemDto
    {
        public string? item { get; set; }
        public string? productName { get; set; }
        public int quantity { get; set; }
        public double unitPrice { get; set; }
        public double subtotal { get; set; }
    }

    #endregion
}
