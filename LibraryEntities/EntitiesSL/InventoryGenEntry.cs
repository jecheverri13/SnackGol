using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class InventoryGenEntry
    {

        [JsonProperty("Series")]
        public string? serialNumber { get; set; }


        [JsonProperty("BPL_IDAssignedToInvoice")]
        public string? branch { get; set; }


        [JsonProperty("DocDate")]
        public string? documentPostingDate { get; set; }


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

        [JsonProperty("U_OK1_SF_EntrySM")]
        public int? documentExitNumber { get; set; }


        [JsonProperty("DocumentLines")]
        public List<Lines_GenEntry> lines { get; set; }


        public InventoryGenEntry()
        {
            lines = new List<Lines_GenEntry>();
        }
    }



    public class Lines_GenEntry
    {
        [JsonProperty("ItemCode")]
        public string? itemCode { get; set; }

        [JsonProperty("ItemDescription")]
        public string? itemDescription { get; set; }

        [JsonProperty("Quantity")]
        public int? quantity { get; set; }

        [JsonProperty("WarehouseCode")]
        public string? warehouseCode { get; set; }

        [JsonProperty("UnitPrice")]
        public double? unitPrice { get; set; }

        [JsonProperty("CostingCode")]
        public string? costingCode { get; set; }

        [JsonProperty("CostingCode2")]
        public string? costingCode2 { get; set; }

        [JsonProperty("CostingCode3")]
        public string? costingCode3 { get; set; }

        [JsonProperty("CostingCode4")]
        public string? costingCode4 { get; set; }

        [JsonProperty("ProjectCode")]
        public string? ProjectCode { get; set; }

        [JsonProperty("U_linea")]
        public int? lineDocumentOrigin { get; set; }
        
        [JsonProperty("SerialNumbers")]
        public List<SerialNumbers_GenEntry> serialNumbers { get; set; }

        public Lines_GenEntry()
        {
            serialNumbers = new List<SerialNumbers_GenEntry>();
        }
    }


    public class SerialNumbers_GenEntry
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
