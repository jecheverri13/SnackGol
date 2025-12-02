using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;

namespace MSSnackGolFrontend.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductoController(IHttpClientFactory httpClientFactory)
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

        private HttpClient CreateApiClient()
        {
            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);
            return client;
        }

        private async Task<ProductDto?> FetchProductAsync(int id)
        {
            try
            {
                var client = CreateApiClient();
                var resp = await client.GetFromJsonAsync<Response<ProductDto>>($"api/ProductManagment/{id}");
                return resp?.response;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = CreateApiClient();
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

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductCreateRequest();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = CreateApiClient();

            try
            {
                var resp = await client.PostAsJsonAsync("api/ProductManagment/Create", model);
                if (resp.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                // read possible error details and show simple message
                var details = await resp.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"No se pudo crear el producto: {(int)resp.StatusCode} {resp.ReasonPhrase} {details}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API: " + ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateApiClient();

            try
            {
                var resp = await client.GetFromJsonAsync<Response<ProductDto>>($"api/ProductManagment/{id}");
                if (resp == null || !resp.success)
                    return NotFound();

                // mapear a ProductCreateRequest para reutilizar la vista
                var model = new ProductCreateRequest {
                    category_id = resp.response!.category_id,
                    name = resp.response.name,
                    description = resp.response.description,
                    price = resp.response.price,
                    stock = resp.response.stock,
                    image_url = resp.response.image_url,
                    is_active = resp.response.is_active
                };
                ViewData["ProductId"] = id;
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductCreateRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = id;
                return View(model);
            }

            var client = CreateApiClient();

            try
            {
                var resp = await client.PutAsJsonAsync($"api/ProductManagment/{id}", model);
                if (resp.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                var details = await resp.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"No se pudo actualizar el producto: {(int)resp.StatusCode} {resp.ReasonPhrase} {details}");
                ViewData["ProductId"] = id;
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API: " + ex.Message);
                ViewData["ProductId"] = id;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await FetchProductAsync(id);
            if (product == null)
            {
                TempData["CatalogFlash"] = "No encontramos el producto que intentaste abrir.";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await FetchProductAsync(id);
            if (product == null)
            {
                TempData["CatalogFlash"] = "El producto ya no está disponible.";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateApiClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/ProductManagment/{id}");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["CatalogFlash"] = "Producto eliminado correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                var details = await response.Content.ReadAsStringAsync();
                TempData["CatalogFlash"] = $"No pudimos eliminar el registro (HTTP {(int)response.StatusCode}). {details}";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                TempData["CatalogFlash"] = "Error al comunicarse con la API: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}