using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models.PaymentGatewayModels
{
    public class ManagerResponse
    {
        public string? UrlBase { get; set; }
        public string? RedirectAfterPayment { get; set; }
        public string? RedirectMobileOrder { get; set; }
        public string? RedirectCancelMobileOrder { get; set; }
        public string? AuthAdmin { get; set; }
        public string? Payment { get; set; }
        public string? AbortOrder { get; set; }
        public string? ConfirmOrder { get; set; }
        public string? UserName { get; set; }
        public string? Secret { get; set; }
    }
}
