using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryConnection.DbSet
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int category_id { get; set; }

        [Required]
        [MaxLength(150)]
        public string name { get; set; }

        [MaxLength(1000)]
        public string? description { get; set; }

        [Required]
        public double price { get; set; }

        [Required]
        public int stock { get; set; }

        public string? image_url { get; set; }

        public bool is_active { get; set; } = true;

        public Category Category { get; set; }
    }
}
