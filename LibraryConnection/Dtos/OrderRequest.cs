using System.ComponentModel.DataAnnotations;

namespace LibraryConnection.Dtos
{
    public class OrderRequest
    {
        public string? order_id { get; set; }
        public string? customer_id { get; set; }
        public string? resolution_id { get; set; }
        public DateTime order_date { get; set; }
        public string? cufe { get; set; }
        public string? url { get; set; }
        public int? doc_entry_sap { get; set; }
        public string? status_sap { get; set; }
        public string? status_fe { get; set; }
        public string? serial_number { get; set; }
        public string? credit_card_number { get; set; }
        public string? credit_card_expiration { get; set; }
        public string? payment_system { get; set; }
        public string? voucher_number { get; set; }
        public double total_gross_amount { get; set; }
        public double total_net_price { get; set; }
        public List<OrderLineRequest>? order_lines { get; set; }
    }

    /// <summary>
    /// Solicitud para actualizar el estado de un pedido.
    /// </summary>
    public class UpdateStatusRequest
    {
        /// <summary>
        /// Nuevo estado del pedido: Confirmed, Preparing, ReadyForPickup, Delivered
        /// </summary>
        [Required]
        public string? NewStatus { get; set; }
        
        /// <summary>
        /// Usuario/staff que realiza la actualización (opcional).
        /// </summary>
        public string? UpdatedBy { get; set; }
        
        /// <summary>
        /// Si es true, permite retroceder el estado (solo para correcciones).
        /// </summary>
        public bool ForceUpdate { get; set; } = false;
    }

    public class OrderLineRequest
    {
        [StringLength(25)]
        public string? item { get; set; }

        [Range(0.01, double.MaxValue)]
        public string? description { get; set; }
        public double? grossAmount { get; set; }
        public double? netPrice { get; set; }
        public string? taxCode { get; set; }
        public double taxAmount { get; set; }
        [Range(1, double.MaxValue)]
        public double quantity { get; set; }

    }
}