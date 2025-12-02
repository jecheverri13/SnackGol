using System;

namespace MSSnackGolFrontend.Models
{
    public class OrderViewModel
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public int ItemsCount { get; set; }
        public decimal Total { get; set; }
    }
}
