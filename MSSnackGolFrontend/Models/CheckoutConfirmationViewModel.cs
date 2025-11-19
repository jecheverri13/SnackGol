using System;

namespace MSSnackGolFrontend.Models
{
    public class CheckoutConfirmationViewModel
    {
        public string? OrderId { get; set; }
        public string? Status { get; set; }
        public double Total { get; set; }
        public PickupInfo? Pickup { get; set; }

        public sealed class PickupInfo
        {
            public string? Code { get; set; }
            public string? PayloadBase64 { get; set; }
            public string? QrImageBase64 { get; set; }
            public DateTime? GeneratedAtUtc { get; set; }
            public DateTime? DeliveredAtUtc { get; set; }
        }
    }
}
