namespace LibraryConnection.Dtos
{
    public class ProductCreateRequest
    {
        public int category_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? description { get; set; }
        public double price { get; set; }
        public int stock { get; set; }
        public string? image_url { get; set; }
        public bool is_active { get; set; } = true;
    }
}
