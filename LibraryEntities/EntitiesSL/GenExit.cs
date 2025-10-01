using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class GenExit
    {
        //[JsonProperty("CardCode")]
        //public string? cardCode { get; set; }

        [JsonProperty("Series")]
        public string? series { get; set; }

        [JsonProperty("BPL_IDAssignedToInvoice")]
        public string? branch { get; set; }

        [JsonProperty("DocDate")]
        public string? docDate { get; set; }

        [JsonProperty("TaxDate")]
        public string? taxDate { get; set; }

        [JsonProperty("U_OK1_SF_NSOL")]
        public string? document { get; set; }

        [JsonProperty("U_idFallec")]
        public string? deceasedId { get; set; }

        [JsonProperty("U_NomFallec")]
        public string? deceasedName { get; set; }

        [JsonProperty("U_OrigDoc")]
        public string? documentOrigin { get; set; }

        [JsonProperty("DocumentLines")]
        public List<Lines_GenExit> lines { get; set; }

        public GenExit()
        {
            lines = new List<Lines_GenExit>();
        }
    }

    public class Lines_GenExit
    {
        [JsonProperty("LineId")]
        public int? id { get; set; }

        [JsonProperty("ItemCode")]
        public string? itemCode { get; set; }

        [JsonProperty("ItemName")]
        public string? itemName { get; set; }

        [JsonProperty("Quantity")]
        public int? quantity { get; set; }

        [JsonProperty("UnitPrice")]
        public int? price { get; set; }

        [JsonProperty("ProjectCode")]
        public string? ProjectCode { get; set; }

        [JsonProperty("U_RotuloCofre")]
        public string? serialItem { get; set; }

        [JsonProperty("U_almacen")]
        public string? warehouseCode { get; set; }

        [JsonProperty("CostingCode")]
        public string? costingCode { get; set; }

        [JsonProperty("CostingCode2")]
        public string? costingCode2 { get; set; }

        [JsonProperty("CostingCode3")]
        public string? costingCode3 { get; set; }

        [JsonProperty("CostingCode4")]
        public string? costingCode4 { get; set; }

        [JsonProperty("U_linea")]
        public int? lineDocumentOrigin { get; set; }

        [JsonProperty("SerialNumbers")]
        public List<SerialNumbers_GenExit> serialNumbers { get; set; }

        public Lines_GenExit()
        {
            serialNumbers = new List<SerialNumbers_GenExit>();
        }
    }


    public class SerialNumbers_GenExit
    {
        [JsonProperty("BaseLineNumber")]
        public int? baseLineNumber { get; set; }

        [JsonProperty("SystemSerialNumber")]
        public string? systemSerialNumber { get; set; }

        [JsonProperty("InternalSerialNumber")]
        public string? internalSerialNumber { get; set; }

        [JsonProperty("Quantity")]
        public int? quantity { get; set; }

    }
}
