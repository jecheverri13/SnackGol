using System;

namespace LibraryEntities.Models
{
    public class PickupInfoResponse
    {
        public string? orderId { get; set; }
        public string? pickupCode { get; set; }
        public string? pickupPayloadBase64 { get; set; }
        public string? pickupQrImageBase64 { get; set; }
        public DateTime? generatedAtUtc { get; set; }
        public DateTime? deliveredAtUtc { get; set; }
        public string? status { get; set; }
    }

    public class CheckoutResult
    {
        public string orderId { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public double total { get; set; }
        public PickupInfoResponse? pickup { get; set; }
    }

    public class PickupValidationRequest
    {
        public string? token { get; set; }
        public string? verified_by { get; set; }
    }
}
