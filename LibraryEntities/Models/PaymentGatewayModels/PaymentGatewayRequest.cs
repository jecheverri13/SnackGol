using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models.PaymentGatewayModels
{
    public class PaymentGatewayRequest
    {
        public string? MerchantId { get; set; }
        public PaymentGatewayConfigurationRequest PaymentGatewayConfigurationRequest { get; set; }
        public int ManagerId { get; set; }
    }

    public class PaymentGatewayConfigurationRequest
    {
        public string? Connection { get; set; }
        public string? RegisterOrderConnection { get; set; }
        public string? PaymentCancellationConnection { get; set; }
        public string? OrderPaymentConnection { get; set; }
        public string? OrderStatusConnection { get; set; }
        public string? CardStatusConnection { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
