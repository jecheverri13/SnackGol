namespace LibraryConnection.Dtos
{
    public class AddCartItemRequest
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string? session_token { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int quantity { get; set; }
    }

    public class CheckoutRequest
    {
        public string customer_id { get; set; }
        public string? session_token { get; set; }
    }
}