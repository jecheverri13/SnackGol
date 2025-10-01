using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class DocumentTransactional
    {
        [JsonProperty("User")]
        public string? user { get; set; }

        [JsonProperty("CardCode")]
        public string? cardCode { get; set; }

        [JsonProperty("CardName")]
        public string? cardName { get; set; }

        [JsonProperty("Series")]
        public string? series { get; set; }

        [JsonProperty("BPL_IDAssignedToInvoice")]
        public string? branch { get; set; }


        [JsonProperty("DocDate")]
        public string? docDate { get; set; }


        [JsonProperty("U_OK1_SF_NSOL")]
        public string? document { get; set; }


        [JsonProperty("U_idFallec")]
        public string? deceasedId { get; set; }


        [JsonProperty("U_NomFallec")]
        public string? deceasedName { get; set; }


        [JsonProperty("U_OrigDoc")]
        public string? documentOrigin { get; set; }


        [JsonProperty("DocumentLines")]
        public List<Document_Lines_InventoryGenExit> lines { get; set; }


        public DocumentTransactional()
        {
            lines = new List<Document_Lines_InventoryGenExit>();
        }
    }



    public class Document_Lines_InventoryGenExit
    {
        [JsonProperty("LineId")]
        public int? id { get; set; }

        [JsonProperty("ItemCode")]
        public string? itemCode { get; set; }

        [JsonProperty("ItemName")]
        public string? itemName { get; set; }

        [JsonProperty("Quantity")]
        public int? quantity { get; set; }

        [JsonProperty("U_RotuloCofre")]
        public string? serialItem { get; set; }

        [JsonProperty("WarehouseCode")]
        public string? warehouseCode { get; set; }

        [JsonProperty("CostingCode")]
        public string? costingCode { get; set; }

        [JsonProperty("CostingCode2")]
        public string? costingCode2 { get; set; }

        [JsonProperty("CostingCode3")]
        public string? costingCode3 { get; set; }

        [JsonProperty("CostingCode4")]
        public string? costingCode4 { get; set; }

        [JsonProperty("SerialNumbers")]
        public List<SerialNumbers_InventoryGenExit> serialNumbers { get; set; }

        public Document_Lines_InventoryGenExit()
        {
            serialNumbers = new List<SerialNumbers_InventoryGenExit>();
        }
    }


    public class SerialNumbers_InventoryGenExit
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
