using System.ComponentModel.DataAnnotations;

namespace MSSnackGolFrontend.Models
{
    public class PagoViewModel
    {
        /// <summary>
        /// Fecha actual del pago
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Subtotal del pedido (antes de IVA)
        /// </summary>
        [Required(ErrorMessage = "El subtotal es requerido")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// IVA (19% del subtotal)
        /// </summary>
        public decimal IVA => Subtotal * 0.19m;

        /// <summary>
        /// Total a pagar (Subtotal + IVA)
        /// </summary>
        public decimal Total => Subtotal + IVA;

        /// <summary>
        /// Método de pago seleccionado: "nequi" o "tarjeta"
        /// </summary>
        [Required(ErrorMessage = "Debe seleccionar un método de pago")]
        public string MetodoPago { get; set; } = string.Empty;

        /// <summary>
        /// Número de Nequi (si aplica): 10 dígitos comenzando con 3
        /// </summary>
        public string? NumeroCuentaNequi { get; set; }

        /// <summary>
        /// Número de tarjeta de crédito (si aplica)
        /// </summary>
        public string? NumeroTarjeta { get; set; }

        /// <summary>
        /// ID de la orden/carrito asociada
        /// </summary>
        public int? OrderId { get; set; }

        /// <summary>
        /// ID del cliente
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Código de confirmación después del pago
        /// </summary>
        public string? CodigoConfirmacion { get; set; }

        /// <summary>
        /// Estado de la transacción
        /// </summary>
        public string? EstadoTransaccion { get; set; }
    }
}
