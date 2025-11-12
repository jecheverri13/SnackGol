using System.Net.Http.Json;
using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MSSnackGolFrontend.Services
{
    public record CartSummary(int Count, double Total);

    public class CartSummaryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CartSummaryService> _logger;

        public CartSummaryService(IHttpClientFactory httpClientFactory, ILogger<CartSummaryService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<CartSummary> GetAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var token = Infrastructure.SessionTokenHelper.GetOrCreate(httpContext);
            client.DefaultRequestHeaders.Remove("X-Session-Token");
            client.DefaultRequestHeaders.Add("X-Session-Token", token);

            try
            {
                var response = await client.GetAsync("api/CartManagment", cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Cart summary request returned {StatusCode}", response.StatusCode);
                    return new CartSummary(0, 0);
                }

                var payload = await response.Content.ReadFromJsonAsync<Response<CartDto>>(cancellationToken: cancellationToken);
                var cart = payload?.response;
                if (cart is null)
                {
                    return new CartSummary(0, 0);
                }

                return new CartSummary(cart.count, cart.total);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cart summary request failed");
                return new CartSummary(0, 0);
            }
        }
    }
}
