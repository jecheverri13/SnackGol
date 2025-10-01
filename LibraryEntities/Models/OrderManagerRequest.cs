using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class OrderManagerResponse
    {
        public string? TransactionId { get; set; }
        public string? MerchantId { get; set; }
        public int RequestedAmount { get; set; }
        public string? Currency { get; set; }
        public string? ReceiptId { get; set; }
        public string? OrderUuid { get; set; }
        public string? DeliveryType { get; set; }
        public string? RedirectHost { get; set; }
    }

    public class CancelOrderManagerResponse
    {
        public string? TransactionId { get; set; }
        public string? MerchantId { get; set; }
        public int RequestedAmount { get; set; }
        public string? Currency { get; set; }
        public string? receiptId { get; set; }
    }
}
