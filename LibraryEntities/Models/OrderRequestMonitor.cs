using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class OrderRequestMonitor
    {
        
        public PaginationParams pagination { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string? status_sap { get; set; }
        public string? status_fe { get; set; }

    }
}
