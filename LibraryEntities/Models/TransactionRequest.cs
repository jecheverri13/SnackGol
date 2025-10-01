using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class TransactionRequest
    {
        [Key]
        public string? transactionId { get; set; }
        public string? merchantId { get; set; }
        public string? externalOrderId { get; set; }
        public string? orderId { get; set; }
        public string? receiptId { get; set; }
        public int status { get; set; } = 0; 
        public DateTime orderDate { get; set; }
        public int amount { get; set; }
        public string? creditCardNumber { get; set; }
        public string? creditCardExpiration { get; set; }
        public string? paymentSystem { get; set; }
        public string? cardHolderName { get; set; }
        public string? email { get; set; }
    }
}
