using System;
using System.Collections.Generic;

namespace MSSnackGolFrontend.Models
{
    public class FacturaItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Total => PrecioUnitario * Cantidad;
    }

    public class FacturaViewModel
    {
        public string NumeroFactura { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string ClienteNombre { get; set; } = "";
        public string Email { get; set; } = "";
        public List<FacturaItem> Items { get; set; } = new();
        public decimal Subtotal => CalculateSubtotal();
        public decimal Impuesto { get; set; } = 0m;
        public decimal Total => Subtotal + Impuesto;

        private decimal CalculateSubtotal()
        {
            decimal s = 0m;
            foreach (var it in Items) s += it.Total;
            return s;
        }
    }
}
