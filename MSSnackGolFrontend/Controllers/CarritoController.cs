using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;

namespace MSSnackGolFrontend.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CarritoController> _logger;

        public CarritoController(IHttpClientFactory httpClientFactory, ILogger<CarritoController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private string GetOrCreateSessionToken()
        {
            const string key = "SessionToken";
            if (!HttpContext.Session.TryGetValue(key, out _))
            {
                var token = Guid.NewGuid().ToString("N");
                HttpContext.Session.SetString(key, token);
            }
            return HttpContext.Session.GetString(key)!;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            var vm = new Models.CartPageViewModel();
            try
            {
                // Llamada manual para soportar dos shapes que la API podría devolver:
                // 1) Response<List<ProductDto>> -> response = [ ... ]
                // 2) Response<dynamic> con response = { items: [...], meta: {...} }
                var http = await client.GetAsync("api/ProductManagment/List");
                _logger.LogDebug("Product list request returned {StatusCode}", http.StatusCode);
                if (http.IsSuccessStatusCode)
                {
                    var json = await http.Content.ReadAsStringAsync();
                    _logger.LogDebug("Product list payload length: {Len}", json?.Length ?? 0);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        try
                        {
                            // Usar JsonDocument para inspeccionar la forma y mapear manualmente los items
                            using var doc = System.Text.Json.JsonDocument.Parse(json);
                            var root = doc.RootElement;
                            System.Text.Json.JsonElement itemsEl = default;
                            bool foundItems = false;

                            if (root.ValueKind == System.Text.Json.JsonValueKind.Object && root.TryGetProperty("response", out var respEl))
                            {
                                // response can be array or object with items
                                if (respEl.ValueKind == System.Text.Json.JsonValueKind.Array)
                                {
                                    itemsEl = respEl;
                                    foundItems = true;
                                }
                                else if (respEl.ValueKind == System.Text.Json.JsonValueKind.Object && respEl.TryGetProperty("items", out var it))
                                {
                                    itemsEl = it;
                                    foundItems = true;
                                }
                            }
                            else if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                            {
                                itemsEl = root;
                                foundItems = true;
                            }

                            if (foundItems && itemsEl.ValueKind == System.Text.Json.JsonValueKind.Array)
                            {
                                var list = new List<ProductDto>();
                                foreach (var el in itemsEl.EnumerateArray())
                                {
                                    try
                                    {
                                        var p = new ProductDto
                                        {
                                            id = el.TryGetProperty("id", out var idEl) && idEl.ValueKind != System.Text.Json.JsonValueKind.Null ? idEl.GetInt32() : 0,
                                            category_id = el.TryGetProperty("category_id", out var catEl) && catEl.ValueKind != System.Text.Json.JsonValueKind.Null ? catEl.GetInt32() : 0,
                                            name = el.TryGetProperty("name", out var nameEl) && nameEl.ValueKind != System.Text.Json.JsonValueKind.Null ? nameEl.GetString() ?? string.Empty : string.Empty,
                                            description = el.TryGetProperty("description", out var descEl) && descEl.ValueKind != System.Text.Json.JsonValueKind.Null ? descEl.GetString() : null,
                                            price = el.TryGetProperty("price", out var priceEl) && priceEl.ValueKind != System.Text.Json.JsonValueKind.Null ? priceEl.GetDouble() : 0.0,
                                            stock = el.TryGetProperty("stock", out var stockEl) && stockEl.ValueKind != System.Text.Json.JsonValueKind.Null ? stockEl.GetInt32() : 0,
                                            image_url = el.TryGetProperty("image_url", out var imgEl) && imgEl.ValueKind != System.Text.Json.JsonValueKind.Null ? imgEl.GetString() : null,
                                            is_active = el.TryGetProperty("is_active", out var activeEl) && activeEl.ValueKind == System.Text.Json.JsonValueKind.True
                                        };
                                        list.Add(p);
                                    }
                                    catch (Exception itemEx)
                                    {
                                        // log per-item parse issues and continue
                                        _logger.LogWarning(itemEx, "Error parseando un producto dentro de items: {Snippet}", el.GetRawText().Length > 200 ? el.GetRawText().Substring(0, 200) + "…" : el.GetRawText());
                                    }
                                }
                                vm.Products = list;
                            }
                            else
                            {
                                vm.Products = new List<ProductDto>();
                                // log snippet to help debugging
                                var snippet = json.Length > 1000 ? json.Substring(0, 1000) + "…" : json;
                                _logger.LogWarning("No se encontraron items en la respuesta de productos. JSON snippet: {Snippet}", snippet);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log a capped snippet of the JSON to help diagnose schema mismatches
                            var snippet = json?.Length > 1000 ? json.Substring(0, 1000) + "…" : json;
                            _logger.LogWarning(ex, "Error procesando JSON de productos. JSON snippet: {Snippet}", snippet);
                            vm.Products = new List<ProductDto>();
                        }
                    }
                    else
                    {
                        vm.Products = new List<ProductDto>();
                    }
                }
                else if (http.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    vm.Products = new List<ProductDto>();
                }
                else
                {
                    vm.Products = new List<ProductDto>();
                }
            }
            catch
            {
                // Fallback: catálogo vacío si API no responde
                vm.Products = new List<ProductDto>();
            }

            try
            {
                var cartHttp = await client.GetAsync("api/CartManagment");
                if (cartHttp.IsSuccessStatusCode)
                {
                    var cartResp = await cartHttp.Content.ReadFromJsonAsync<Response<CartDto>>();
                    vm.Cart = cartResp?.response ?? vm.Cart;
                }
            }
            catch
            {
                // Mantener carrito vacío
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SampleData()
        {
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            try
            {
                var resp = await client.PostAsync("api/CartManagment/Sample", null);
                if (resp.IsSuccessStatusCode)
                {
                    TempData["CartFlash"] = "Datos de muestra agregados al carrito.";
                }
                else
                {
                    string details = string.Empty;
                    try
                    {
                        var body = await resp.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(body))
                        {
                            details = body.Length > 220 ? body.Substring(0, 220) + "…" : body;
                        }
                    }
                    catch { /* ignore read body errors */ }
                    TempData["CartFlash"] = $"No se pudo cargar datos de muestra: {(int)resp.StatusCode} {resp.ReasonPhrase}. {details}";
                }
            }
            catch
            {
                TempData["CartFlash"] = "No se pudo comunicar con la API para cargar datos de muestra.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int productId, int quantity = 1)
        {
            if (quantity <= 0) quantity = 1;
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            var body = new AddCartItemRequest { product_id = productId, quantity = quantity, session_token = token };
            await client.PostAsJsonAsync("api/CartManagment/Items", body);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(int id, int quantity)
        {
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            var body = new UpdateCartItemRequest { quantity = quantity };
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/CartManagment/Items/{id}")
            {
                Content = JsonContent.Create(body)
            };
            await client.SendAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);
            await client.DeleteAsync($"api/CartManagment/Items/{id}");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            var body = new { session_token = token };
            await client.PostAsJsonAsync("api/OrderManagement/Checkout", body);
            return RedirectToAction(nameof(Index));
        }
    }
}