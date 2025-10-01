using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class PaymentEcommerceJson
    {
        public int? Sucursal { get; set; }
        public string? ClienteID { get; set; }
        public string? FechaEntrega { get; set; }
        public List<DocumentLineEcommerce>? DocumentLines { get; set; }
        public int? NroReferencia { get; set; }
        public int? SeriesFactura { get; set; }
        public List<DireccionEntregaEcommerce>? DireccionEntrega { get; set; }
        public string? FechaContabilizacion { get; set; }
    }

    public class DocumentLineEcommerce
    {
        public string? Almacen { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Descuento { get; set; }
        public string? ArticuloCodigo { get; set; }
        public string? CodigoImpuesto { get; set; }
        public decimal? PrecioUnitario { get; set; }
    }

    public class DireccionEntregaEcommerce
    {
        public string? Name { get; set; }
        public string? Direccion { get; set; }
        public string? Municipio { get; set; }
        public string? Departamento { get; set; }
    }
}
