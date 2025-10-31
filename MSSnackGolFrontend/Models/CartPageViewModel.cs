using LibraryConnection.Dtos;

namespace MSSnackGolFrontend.Models
{
    public class CartPageViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public CartDto Cart { get; set; } = new() { items = new List<CartItemDto>(), total = 0, count = 0 };
    }
}
