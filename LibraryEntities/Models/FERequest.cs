using Newtonsoft.Json;
using System.Collections.Generic;

namespace LibraryEntities.Models
{

    public class FERequest
    {
        [JsonProperty("Factura")]
        public Factura Factura { get; set; }
    }

    public class Factura
    {
        [JsonProperty("Cabecera")]
        public Cabecera Cabecera { get; set; }

        [JsonProperty("NumeracionDIAN")]
        public NumeracionDIAN NumeracionDIAN { get; set; }

        [JsonProperty("Emisor")]
        public Emisor Emisor { get; set; }

        [JsonProperty("Notificacion")]
        public Notificacion Notificacion { get; set; }

        [JsonProperty("Cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("MediosDePago")]
        public MediosDePago MediosDePago { get; set; }

        [JsonProperty("TasaDeCambio")]
        public TasaDeCambio TasaDeCambio { get; set; }

        [JsonProperty("Impuestos")]
        public List<Impuesto> Impuestos { get; set; }

        [JsonProperty("Descuento")]
        public Descuento Descuento { get; set; }

        [JsonProperty("Totales")]
        public Totales Totales { get; set; }

        [JsonProperty("Linea")]
        public List<Linea> Linea { get; set; }
    }

    public class Cabecera
    {
        public string Numero { get; set; }
        public string FechaEmision { get; set; }
        public string Vencimiento { get; set; }
        public string HoraEmision { get; set; }
        public string MonedaFactura { get; set; }
        public string Observaciones { get; set; }
        public string TipoFactura { get; set; }
        public string FormaDePago { get; set; }
        public string TipoOperacion { get; set; }
        public string OrdenCompra { get; set; }
        public string Ambiente { get; set; }
        public string LineasDeFactura { get; set; }
    }

    public class NumeracionDIAN
    {
        public string NumeroResolucion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string PrefijoNumeracion { get; set; }
        public string ConsecutivoInicial { get; set; }
        public string ConsecutivoFinal { get; set; }
    }

    public class Emisor
    {
        public string Caja { get; set; }
        public string IDPlugin { get; set; }
        public string TipoPersona { get; set; }
        public string TipoRegimen { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string DV { get; set; }
        public string RazonSocial { get; set; }
        public string NumeroMatriculaMercantil { get; set; }
        public string NombreComercial { get; set; }
        public Direccion Direccion { get; set; }
        public List<Obligacion> ObligacionesEmisor { get; set; }
        public Direccion DireccionFiscal { get; set; }
        public List<Tributo> TributoEmisor { get; set; }
        public Contacto Contacto { get; set; }
    }

    public class Direccion
    {
        public string CodigoMunicipio { get; set; }
        public string NombreCiudad { get; set; }
        public string CodigoPostal { get; set; }
        public string NombreDepartamento { get; set; }
        public string CodigoDepartamento { get; set; }
        public string DireccionProp { get; set; } // cambio de nombre para evitar conflicto con clase

        [JsonProperty("Direccion")]
        public string DireccionJson { set => DireccionProp = value; }
    }

    public class Obligacion
    {
        public string CodigoObligacion { get; set; }
    }

    public class Tributo
    {
        public string CodigoTributo { get; set; }
        public string NombreTributo { get; set; }
    }

    public class Contacto
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }

    public class Notificacion
    {
        public string Tipo { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
    }

    public class Cliente
    {
        public string TipoPersona { get; set; }
        public string TipoRegimen { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string DV { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string NumeroMatriculaMercantil { get; set; }
        public PersonaNatural PersonaNatural { get; set; }
        public Direccion Direccion { get; set; }
        public List<Obligacion> ObligacionesCliente { get; set; }
        public Contacto Contacto { get; set; }
        public List<Tributo> TributoCliente { get; set; }
    }

    public class PersonaNatural
    {
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string Apellido { get; set; }
    }

    public class MediosDePago
    {
        public string CodigoMedioPago { get; set; }
    }

    public class TasaDeCambio
    {
        public string MonedaDestino { get; set; }
        public string ValorTasaDeCambio { get; set; }
        public string FechaTasaDeCambio { get; set; }
    }

    public class Impuesto
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; }
        public string ValorBase { get; set; }
        public string Porcentaje { get; set; }
        public string ValorSubtotal { get; set; }
        public string CodigoUnidadMedidaBase { get; set; }
        public string Redondeo { get; set; }
    }

    public class Descuento
    {
        public string Porcentaje { get; set; }
        public string Valor { get; set; }
        public string ValorBase { get; set; }
    }

    public class Totales
    {
        public string Bruto { get; set; }
        public string BaseImponible { get; set; }
        public string BrutoMasImpuestos { get; set; }
        public string Impuestos { get; set; }
        public string Descuentos { get; set; }
        public string Retenciones { get; set; }
        public string Cargos { get; set; }
        public string General { get; set; }
        public string Redondeo { get; set; }
    }

    public class Linea
    {
        public Detalle Detalle { get; set; }
        public Descuento Descuento { get; set; }
        public Impuesto Impuesto { get; set; }
        public CodificacionEstandar CodificacionEstandar { get; set; }
    }

    public class Detalle
    {
        public string NumeroLinea { get; set; }
        public string Nota { get; set; }
        public string Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public string SubTotalLinea { get; set; }
        public string Descripcion { get; set; }
        public string CantidadBase { get; set; }
        public string UnidadCantidadBase { get; set; }
        public string PrecioUnitario { get; set; }
    }

    public class CodificacionEstandar
    {
        public string CodigoArticulo { get; set; }
        public string CodigoEstandar { get; set; }
    }

}
