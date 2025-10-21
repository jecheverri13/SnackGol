using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryConnection.DbSet
{
    public class Order
    {
        [Key]
        public virtual string? order_id { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public string? customer_id { get; set; }
        public DateTime order_date { get; set; }

        public string? status { get; set; }

        [Required]
        public double total_gross_amount { get; set; }
        [Required]
        public double total_net_price { get; set; }
        public Client client { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
    }

    public class OrderLine
    {
        [Key]
        public int lineNum { get; set; }
        [ForeignKey("Order")]
        public string? orderId { get; set; }

        [StringLength(25)]
        public string? item { get; set; }
        [StringLength(100)]
        public string? description { get; set; }
        public double? gross_amount { get; set; }
        public double? net_price { get; set; }
        public double tax_amount { get; set; }
        [Required]
        public double quantity { get; set; }

        //public Order Order { get; set; }
    }
}
