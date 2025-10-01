using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class OrderCredibancoRequest
    {
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? returnUrl { get; set; }
        public string? orderNumber { get; set; }
        public int amount { get; set; }
    }
}
