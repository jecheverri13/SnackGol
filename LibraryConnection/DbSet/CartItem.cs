using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace LibraryConnection.DbSet
{
    public class CartItem
    {
        [Key]
        public int id { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public Guid cart_id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int product_id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        public double unit_price { get; set; }

        [Required]
        public double subtotal { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
