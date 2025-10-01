using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models.PaymentGatewayModels
{
    public class ManagerRequest
    {
        public string? UrlBase { get; set; }
        public string? UserName { get; set; }
        public string? Secret { get; set; }
    }
}
