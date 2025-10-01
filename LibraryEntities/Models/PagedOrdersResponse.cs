using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class PagedOrdersResponse
    {
        public int Total { get; set; }
        public List<OrderResponseMonitor> Orders { get; set; }
    }
}
