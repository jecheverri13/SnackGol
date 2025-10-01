using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class OrderSummaryRequest
    {
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
    }
}
