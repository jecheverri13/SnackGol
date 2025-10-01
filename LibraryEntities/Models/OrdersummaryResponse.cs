using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class OrdersummaryResponse
    {
        public int TotalDocuments { get; set; }
        public int Accepted { get; set; }
        public int Errors { get; set; }
        public int Pending { get; set; }
    }
}
