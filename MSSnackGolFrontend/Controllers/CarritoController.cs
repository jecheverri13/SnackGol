using LibraryConnection.Dtos;
using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Infrastructure;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MSSnackGolFrontend.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CarritoController> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public CarritoController(IHttpClientFactory httpClientFactory, ILogger<CarritoController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetOrCreateSessionToken();
            using var client = CreateApiClient(token);

            var vm = new Models.CartPageViewModel
            {
                Products = await FetchProductsAsync(client)
            };

            var cart = await FetchCartAsync(client);
            if (cart is not null)
            {
                vm.Cart = cart;
                ViewData["CartSummaryCount"] = cart.count;
                ViewData["CartSummaryTotal"] = cart.total;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SampleData()
        {
            var token = GetOrCreateSessionToken();
            using var client = CreateApiClient(token);

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
            using var client = CreateApiClient(token);

            var body = new AddCartItemRequest { product_id = productId, quantity = quantity, session_token = token };
            await client.PostAsJsonAsync("api/CartManagment/Items", body);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(int id, int quantity)
        {
            var token = GetOrCreateSessionToken();
            using var client = CreateApiClient(token);

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
            using var client = CreateApiClient(token);
            await client.DeleteAsync($"api/CartManagment/Items/{id}");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var token = GetOrCreateSessionToken();
            using var client = CreateApiClient(token);

            var body = new { session_token = token };
            try
            {
                var response = await client.PostAsJsonAsync("api/OrderManagement/Checkout", body);
                if (response.IsSuccessStatusCode)
                {
                    var pickupCode = GeneratePickupCode();
                    TempData["CartFlash"] = "Pedido confirmado. Presenta el código QR en la zona de entrega.";
                    TempData["PickupCode"] = pickupCode;
                    TempData["PickupQrPayload"] = BuildPickupQrPayload(pickupCode, token);
                }
                else
                {
                    string details = await ReadSafeContentAsync(response);
                    TempData["CartFlash"] = $"No se pudo completar el pago: {(int)response.StatusCode} {response.ReasonPhrase}. {details}".Trim();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Checkout failed");
                TempData["CartFlash"] = "No se pudo comunicar con la API de pedidos. Inténtalo de nuevo.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<ProductDto>> FetchProductsAsync(HttpClient client)
        {
            try
            {
                var http = await client.GetAsync("api/ProductManagment/List");
                _logger.LogDebug("Product list request returned {StatusCode}", http.StatusCode);

                if (!http.IsSuccessStatusCode)
                {
                    return new List<ProductDto>();
                }

                var json = await http.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<ProductDto>();
                }

                return ParseProducts(json);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo obtener el listado de productos");
                return new List<ProductDto>();
            }
        }

        private async Task<CartDto?> FetchCartAsync(HttpClient client)
        {
            try
            {
                var http = await client.GetAsync("api/CartManagment");
                _logger.LogDebug("Cart request returned {StatusCode}", http.StatusCode);

                if (!http.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await http.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("response", out var responseEl) && responseEl.ValueKind == JsonValueKind.Object)
                {
                    return JsonSerializer.Deserialize<CartDto>(responseEl.GetRawText(), _jsonOptions);
                }

                return JsonSerializer.Deserialize<CartDto>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo obtener el carrito actual");
                return null;
            }
        }

        private List<ProductDto> ParseProducts(string json)
        {
            var products = new List<ProductDto>();

            try
            {
                using var doc = JsonDocument.Parse(json);
                if (!TryExtractItemsArray(doc.RootElement, out var itemsElement))
                {
                    return products;
                }

                foreach (var element in itemsElement.EnumerateArray())
                {
                    try
                    {
                        var product = new ProductDto
                        {
                            id = element.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idValue) ? idValue : 0,
                            category_id = element.TryGetProperty("category_id", out var catEl) && catEl.TryGetInt32(out var catValue) ? catValue : 0,
                            name = element.TryGetProperty("name", out var nameEl) && nameEl.ValueKind != JsonValueKind.Null ? nameEl.GetString() ?? string.Empty : string.Empty,
                            description = element.TryGetProperty("description", out var descEl) && descEl.ValueKind != JsonValueKind.Null ? descEl.GetString() : null,
                            price = element.TryGetProperty("price", out var priceEl) && priceEl.TryGetDouble(out var priceValue) ? priceValue : 0d,
                            stock = element.TryGetProperty("stock", out var stockEl) && stockEl.TryGetInt32(out var stockValue) ? stockValue : 0,
                            image_url = element.TryGetProperty("image_url", out var imgEl) && imgEl.ValueKind != JsonValueKind.Null ? imgEl.GetString() : null,
                            is_active = element.TryGetProperty("is_active", out var activeEl) && activeEl.ValueKind == JsonValueKind.True
                        };

                        products.Add(product);
                    }
                    catch (Exception itemEx)
                    {
                        var snippet = element.GetRawText();
                        if (snippet.Length > 200)
                        {
                            snippet = snippet.Substring(0, 200) + "…";
                        }
                        _logger.LogWarning(itemEx, "Error procesando un producto. Fragmento: {Snippet}", snippet);
                    }
                }
            }
            catch (Exception ex)
            {
                var snippet = json.Length > 1000 ? json.Substring(0, 1000) + "…" : json;
                _logger.LogWarning(ex, "Error procesando la respuesta de productos. JSON: {Snippet}", snippet);
            }

            return products;
        }

        private static bool TryExtractItemsArray(JsonElement root, out JsonElement itemsElement)
        {
            if (root.ValueKind == JsonValueKind.Array)
            {
                itemsElement = root;
                return true;
            }

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("response", out var responseEl))
                {
                    if (responseEl.ValueKind == JsonValueKind.Array)
                    {
                        itemsElement = responseEl;
                        return true;
                    }

                    if (responseEl.ValueKind == JsonValueKind.Object && responseEl.TryGetProperty("items", out var nestedItems) && nestedItems.ValueKind == JsonValueKind.Array)
                    {
                        itemsElement = nestedItems;
                        return true;
                    }
                }

                if (root.TryGetProperty("items", out var items) && items.ValueKind == JsonValueKind.Array)
                {
                    itemsElement = items;
                    return true;
                }
            }

            itemsElement = default;
            return false;
        }

        private HttpClient CreateApiClient(string token)
        {
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);
            return client;
        }

        private string GetOrCreateSessionToken() => SessionTokenHelper.GetOrCreate(HttpContext);

        private static string GeneratePickupCode()
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
            return $"SG-{DateTime.UtcNow:yyMMddHHmmss}-{suffix}";
        }

        private string BuildPickupQrPayload(string pickupCode, string sessionToken)
        {
            var payload = new
            {
                tag = "SNACKGOL_DELIVERY",
                code = pickupCode,
                session = sessionToken,
                generatedAt = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(payload, _jsonOptions);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        private static async Task<string> ReadSafeContentAsync(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return string.Empty;
                }

                return content.Length > 200 ? content.Substring(0, 200) + "…" : content;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}