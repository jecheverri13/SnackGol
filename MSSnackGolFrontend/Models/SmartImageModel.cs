namespace MSSnackGolFrontend.Models
{
    public class SmartImageModel
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? image_url { get; set; }
        public int width { get; set; } = 400;
        public int height { get; set; } = 300;
        public string? cssClass { get; set; }
        public string? alt { get; set; }
    }
}
