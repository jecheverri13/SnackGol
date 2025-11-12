using Microsoft.AspNetCore.Mvc;
using MSSnackGolFrontend.Services;

namespace MSSnackGolFrontend.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly CartSummaryService _cartSummaryService;

        public CartSummaryViewComponent(CartSummaryService cartSummaryService)
        {
            _cartSummaryService = cartSummaryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var summary = await _cartSummaryService.GetAsync(HttpContext);
            return View(summary);
        }
    }
}
