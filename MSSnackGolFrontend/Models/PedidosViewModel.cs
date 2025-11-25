namespace MSSnackGolFrontend.Models
{
    /// <summary>
    /// ViewModel para la página de lista de pedidos.
    /// </summary>
    public class PedidosViewModel
    {
        public List<OrderSummaryViewModel> Orders { get; set; } = new();
    }

    /// <summary>
    /// Resumen de un pedido para mostrar en la lista.
    /// </summary>
    public class OrderSummaryViewModel
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = "Confirmed";
        public string StatusDisplay { get; set; } = "Confirmado";
        public string StatusClass { get; set; } = "status-confirmed";
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PickupCode { get; set; }
        public int ItemCount { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();

        /// <summary>
        /// Indica si el pedido está activo (no entregado).
        /// </summary>
        public bool IsActive => !Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Indica si el pedido está listo para recoger.
        /// </summary>
        public bool IsReadyForPickup => Status.Equals("ReadyForPickup", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Item de un pedido.
    /// </summary>
    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Subtotal { get; set; }
    }
}
