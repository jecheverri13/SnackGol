using System.ComponentModel.DataAnnotations;

namespace LibraryEntities.Models
{
    // Modelos adicionales (deberían estar en LibraryConnection.DbSet)
    public class OrderResponse
    {
        [Key]
        public string order_id { get; set; }
        [Required]
        public int resolution_id { get; set; }
        public string? serial_number { get; set; }
        [Required]
        public string? customer_id { get; set; }
        [Required]
        public DateTime order_date { get; set; }
        [Required]
        public string? status_sap { get; set; }
        public string? status_fe { get; set; }

        public string? credit_card_number { get; set; }
        public string? credit_card_expiration { get; set; }
        public string? payment_system { get; set; }
        public string? voucher_number { get; set; }

        public string? prefix { get; set; }
        [Required]
        public double total_gross_amount { get; set; }
        [Required]
        public double total_net_price { get; set; }
        public string? cufe { get; set; }
        public string? url { get; set; }
        public int? doc_entry_sap { get; set; }
        public ResolutionResponse Resolution { get; set; } 

        public List<OrderLineResponse>? OrderLines { get; set; }
    }

    public class OrderLineResponse
    {
        [Key]
        public int lineNum { get; set; }

        [Required]
        [StringLength(25)]
        public string? item { get; set; }
        public string? description { get; set; }
        [Required]
        public double? grossAmount { get; set; }
        [Required]
        public double? netPrice { get; set; }
        public string? tax_code { get; set; }
        public double? taxAmount { get; set; }
        [Required]
        public double? quantity { get; set; }
    }
}
