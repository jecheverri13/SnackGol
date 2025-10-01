using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class PaymentCancelOrderRequest
    {
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? orderId { get; set; }
        public string? orderNumber { get; set; }
        public int amount { get; set; }
    }
}
