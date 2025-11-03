using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConnection.DbSet
{
    public class Category
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        public bool is_active { get; set; } = true;

        public ICollection<Product> products { get; set; }
    }
}
