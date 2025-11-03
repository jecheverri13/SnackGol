using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConnection.DbSet
{
    public class SalesPoint
    {
        [Key]
        public virtual int id { get; set; }
        public required string name { get; set; }
        public required string location { get; set; }
        public bool is_active { get; set; }
    }
}
