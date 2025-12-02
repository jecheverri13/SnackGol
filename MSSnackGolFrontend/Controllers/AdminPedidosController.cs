using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using LibraryEntities.Models;
using MSSnackGolFrontend.Models;

namespace MSSnackGolFrontend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPedidosController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<AdminPedidosController> _logger;

        public AdminPedidosController(IHttpClientFactory httpFactory, ILogger<AdminPedidosController> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        [HttpGet]
        [Route("/AdminPedidos")]
        public async Task<IActionResult> Index([FromQuery] string? window)
        {
            try
            {
                var client = _httpFactory.CreateClient("Api");
                // Attach JWT from authenticated user's cookie to call API (Authorization: Bearer <token>)
                var accessToken = User?.Claims?.FirstOrDefault(c => c.Type == "access_token")?.Value;
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }
                var effectiveWindow = string.IsNullOrWhiteSpace(window) ? "60m" : window;
                var response = await client.GetAsync($"api/OrderManagement/New?window={effectiveWindow}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorFlash"] = $"Error al consultar pedidos: {response.StatusCode}";
                    return View((object)"[]");
                }

                var content = await response.Content.ReadAsStringAsync();

                // Parse response JSON robustly to tolerate different property namings
                List<object> shaped = new List<object>();
                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;
                    if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("response", out var respEl) && respEl.ValueKind == JsonValueKind.Object)
                    {
                        if (respEl.TryGetProperty("data", out var dataEl) && dataEl.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in dataEl.EnumerateArray())
                            {
                                string orderIdVal = string.Empty;
                                if (item.TryGetProperty("orderId", out var p)) orderIdVal = p.GetString() ?? string.Empty;
                                else if (item.TryGetProperty("order_id", out var p2)) orderIdVal = p2.GetString() ?? string.Empty;

                                string statusVal = item.TryGetProperty("status", out var ps) ? (ps.GetString() ?? string.Empty) : string.Empty;

                                DateTime? orderDateVal = null;
                                if (item.TryGetProperty("orderDate", out var pd))
                                {
                                    if (pd.ValueKind == JsonValueKind.String && DateTime.TryParse(pd.GetString(), out var dt)) orderDateVal = dt;
                                    else if (pd.ValueKind == JsonValueKind.Number && pd.TryGetDateTime(out var dt2)) orderDateVal = dt2;
                                }
                                else if (item.TryGetProperty("order_date", out var pd2))
                                {
                                    if (pd2.ValueKind == JsonValueKind.String && DateTime.TryParse(pd2.GetString(), out var dt3)) orderDateVal = dt3;
                                }

                                int itemsCountVal = 0;
                                if (item.TryGetProperty("itemCount", out var pic) && pic.ValueKind == JsonValueKind.Number && pic.TryGetInt32(out var ic)) itemsCountVal = ic;
                                else if (item.TryGetProperty("items", out var itemsEl) && itemsEl.ValueKind == JsonValueKind.Array) itemsCountVal = itemsEl.GetArrayLength();
                                else if (item.TryGetProperty("item_count", out var pic2) && pic2.ValueKind == JsonValueKind.Number && pic2.TryGetInt32(out var ic2)) itemsCountVal = ic2;

                                decimal? totalVal = null;
                                if (item.TryGetProperty("total", out var pt))
                                {
                                    if (pt.ValueKind == JsonValueKind.Number && pt.TryGetDecimal(out var d)) totalVal = d;
                                    else if (pt.ValueKind == JsonValueKind.String && decimal.TryParse(pt.GetString(), out var d2)) totalVal = d2;
                                }
                                else if (item.TryGetProperty("total_net_price", out var ptn) && ptn.ValueKind == JsonValueKind.Number && ptn.TryGetDecimal(out var dn)) totalVal = dn;

                                shaped.Add(new {
                                    orderId = orderIdVal,
                                    status = statusVal,
                                    orderDate = orderDateVal,
                                    itemsCount = itemsCountVal,
                                    total = totalVal
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing JSON response from API");
                    TempData["ErrorFlash"] = "No se pudo obtener la lista de pedidos";
                    return View((object)"[]");
                }

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(shaped, jsonOptions);
                return View((object)json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en AdminPedidos/Index");
                TempData["ErrorFlash"] = "Error interno al obtener pedidos";
                return View((object)"[]");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/AdminPedidos/UpdateEstado")]
        public async Task<IActionResult> UpdateEstado(string orderId, string? selectedNext)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                TempData["ErrorFlash"] = "Pedido inválido";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(selectedNext))
            {
                TempData["ErrorFlash"] = "Seleccione el siguiente estado";
                return RedirectToAction("Index");
            }

            var next = selectedNext;

            try
            {
                var client = _httpFactory.CreateClient("Api");
                var payload = new
                {
                    NewStatus = next,
                    UpdatedBy = "admin-ui",
                    ForceUpdate = false
                };
                var json = JsonSerializer.Serialize(payload);
                var req = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/OrderManagement/{orderId}/status")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                // Attach JWT token if available
                var access = User?.Claims?.FirstOrDefault(c => c.Type == "access_token")?.Value;
                if (!string.IsNullOrWhiteSpace(access))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access);
                }
                var resp = await client.SendAsync(req);
                var body = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    TempData["ErrorFlash"] = $"Error actualizando estado: {resp.StatusCode}";
                    return RedirectToAction("Index");
                }

                // Attempt to parse response wrapper
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                try
                {
                    var wrapped = JsonSerializer.Deserialize<Response<OrderAdminDto>>(body, options);
                    if (wrapped != null && wrapped.success)
                    {
                        TempData["SuccessFlash"] = "Estado actualizado correctamente";
                    }
                    else
                    {
                        TempData["ErrorFlash"] = wrapped?.errors ?? "Respuesta inválida del servidor";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo deserializar respuesta de actualización, asumiendo éxito si status HTTP OK");
                    TempData["SuccessFlash"] = "Estado actualizado";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en UpdateEstado");
                TempData["ErrorFlash"] = "Error interno al actualizar estado";
                return RedirectToAction("Index");
            }
        }

        private string? CalculateNextState(string? current)
        {
            if (string.IsNullOrWhiteSpace(current)) return "Preparing"; // assume starting from Confirmed
            return current switch
            {
                "Confirmed" => "Preparing",
                "Preparing" => "ReadyForPickup",
                "ReadyForPickup" => "Delivered",
                _ => null
            };
        }

        // Local DTOs for deserialization
        private class OrderAdminDto
        {
            public string? order_id { get; set; }
            public string? status { get; set; }
            public DateTime? order_date { get; set; }
            public decimal? total_gross_amount { get; set; }
            public decimal? total_net_price { get; set; }
            public List<OrderLineDto>? OrderLines { get; set; }
        }

        private class OrderLineDto
        {
            public int? quantity { get; set; }
        }

        private class NewResult
        {
            public int totalCount { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public List<OrderAdminDto>? data { get; set; }
        }

        // Note: the view model `OrderViewModel` is now defined in `MSSnackGolFrontend.Models.OrderViewModel`.
    }
}
