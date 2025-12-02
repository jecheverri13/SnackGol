using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace MSSnackGolFrontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("/Account/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Collect model binding / validation errors and show them in the view
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors != null && kvp.Value.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value.Errors.Select(err => new { Key = kvp.Key, Error = err.ErrorMessage, Exception = err.Exception?.Message }))
                    .ToList();

                var messages = errors.Select(e => string.IsNullOrWhiteSpace(e.Error) ? (e.Exception ?? "Valor no válido") : e.Error + (string.IsNullOrEmpty(e.Exception) ? string.Empty : $" ({e.Exception})")).ToList();
                model.ErrorMessage = string.Join(" | ", messages);

                return View(model);
            }

            var client = _httpClientFactory.CreateClient("Api");
            var payload = new { userNname = model.Email, password = model.Password };
            HttpResponseMessage resp;
            try
            {
                resp = await client.PostAsJsonAsync("/api/LoginManagement/login", payload);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor de autenticación.");
                return View(model);
            }

            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
                return View(model);
            }

            using var stream = await resp.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            // Buscamos token en response.token
            if (doc.RootElement.TryGetProperty("response", out var responseEl)
                && responseEl.ValueKind == JsonValueKind.Object
                && responseEl.TryGetProperty("token", out var tokenEl))
            {
                var token = tokenEl.GetString();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim("access_token", token ?? string.Empty)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Guardar el token en TempData para que se pase a la vista
                TempData["Token"] = token;

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
            return View(model);
        }

        // Diagnostic endpoint to confirm form POST reaches the server with form data
        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("/Account/LoginDebug")]
        public IActionResult LoginDebug([FromForm] LoginViewModel model)
        {
            // Return a simple text response showing received values (for local debug only)
            var email = model?.Email ?? "(null)";
            var pass = model?.Password ?? "(null)";
            return Content($"Received: Email={email}; Password={pass}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
