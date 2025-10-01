using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class OrderResponseMonitor
    {
        public string  id { get; set; }
        public string? code { get; set; }
        public DateTime date { get; set; }
        public string client { get; set; }
        public int attempts { get; set; }
        public double amount { get; set; }
        public string status_sap { get; set; }
        public string status_fe { get; set; }

    }
}
