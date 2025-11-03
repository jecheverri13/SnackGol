namespace LibraryConnection.Dtos
{
    public class CartItemDto
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public double unit_price { get; set; }
        public int quantity { get; set; }
        public double subtotal { get; set; }
    }
}