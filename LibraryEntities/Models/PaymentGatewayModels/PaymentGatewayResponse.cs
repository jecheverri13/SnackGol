using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models.PaymentGatewayModels
{
    public class PaymentGatewayResponse
    {
        public string? MerchantId { get; set; }
        public Clients Client { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
        public PaymentGatewayConfigurationResponse PaymentGatewayConfigurationResponse { get; set; }
        public ManagerResponse managerResponse { get; set; }
    }

    public class PaymentGatewayConfigurationResponse
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
