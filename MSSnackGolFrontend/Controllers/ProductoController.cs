using LibraryConnection.Dtos;
using LibraryEntities.Models;
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

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductCreateRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = GetOrCreateSessionToken();
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

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
    }
}