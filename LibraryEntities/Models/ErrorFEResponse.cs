using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class ErrorFEResponse
    {
        public string? OrderId { get; set; }
        public string? Description { get; set; }
        public DateTime ShippingTime { get; set; }
    }
}
