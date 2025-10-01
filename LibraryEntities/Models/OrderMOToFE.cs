using System.ComponentModel.DataAnnotations;

namespace LibraryEntities.Models
{
    public class OrderMOToFE
    {
        [Key]
        public string? order_id { get; set; }

        public string customer_id { get; set; }

        //----------------------------------------------------------|
        public string? doc_num { get; set; }
        public string? prefix { get; set; }
        public string? currency { get; set; } = "COP";
        public string? resolution { get; set; }
        public string? start_date { get; set; }
        public string? end_date { get; set; }
        public string? initial_number { get; set; }
        public string? final_number { get; set; }
        public ClientResponse? Client { get; set; }

        //----------------------------------------------------------|

        [Required]
        public DateTime order_date { get; set; }

        public string? cufe { get; set; }
        public string? credit_card_number { get; set; }
        public string? credit_card_expiration { get; set; }
        public string? voucher_number { get; set; }
        public double total_gross_amount { get; set; }
        public double total_net_price { get; set; }
        public string? status_sap { get; set; }
        public string? status_fe { get; set; }
        public int? doc_entry_sap { get; set; }
        public int? doc_num_sap { get; set; }
        public List<OrderLineMOToFE>? OrderLines { get; set; }
    }

    public class OrderLineMOToFE
    {
        [Key]
        public int lineNum { get; set; }

        [Required]
        [StringLength(25)]
        public string? item { get; set; }

        public string? item_description { get; set; }

        public double? gross_amount { get; set; }

        public double? net_price { get; set; }

        [Required]
        public double? quantity { get; set; }

        [Required]
        public string? tax_code { get; set; }
    }
}
