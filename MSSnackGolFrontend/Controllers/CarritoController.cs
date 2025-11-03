using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;

namespace MSSnackGolFrontend.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarritoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
                var productsResp = await client.GetFromJsonAsync<Response<List<ProductDto>>>("api/ProductManagment/List");
                vm.Products = productsResp?.response ?? new List<ProductDto>();
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

        [HttpPost]
        public async Task<IActionResult> ProcederPago()
        {
            // Obtener carrito actual desde la API usando la misma sesión
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            var vm = new Models.CartPageViewModel();
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
                // ignorar errores de obtención del carrito; mostrar factura vacía
            }

            TempData["CartJson"] = JsonSerializer.Serialize(vm);
            return RedirectToAction("Index", "Factura");
        }
    }
}