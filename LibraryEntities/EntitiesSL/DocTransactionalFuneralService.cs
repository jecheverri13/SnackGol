using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class DocTransactionalFuneralService
    {
        public class InventoryGenExit
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
            public string? orderFuneralServiceNumber { get; set; }


            [JsonProperty("U_idFallec")]
            public string? deceasedId { get; set; }


            [JsonProperty("U_NomFallec")]
            public string? deceasedName { get; set; }


            [JsonProperty("U_OrigDoc")]
            public string? documentOrigin { get; set; }


            [JsonProperty("DocumentLines")]
            public List<Doc_Lines_InventoryGenExit> Document_Lines { get; set; }


            public InventoryGenExit()
            {
                Document_Lines = new List<Doc_Lines_InventoryGenExit>();
            }
        }



        public class Doc_Lines_InventoryGenExit
        {
            [JsonProperty("ItemCode")]
            public string? itemCode { get; set; }

            [JsonProperty("ItemDescription")]
            public string? itemDescription { get; set; }

            [JsonProperty("Quantity")]
            public int? quantity { get; set; }

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

            [JsonProperty("ProjectCode")]
            public string? ProjectCode { get; set; }

            [JsonProperty("U_linea")]
            public int? lineDocumentOrigin { get; set; }

            [JsonProperty("SerialNumbers")]
            public List<SerialNumbers_InventoryGenExit> Serial_Numbers { get; set; }

            public Doc_Lines_InventoryGenExit()
            {
                Serial_Numbers = new List<SerialNumbers_InventoryGenExit>();
            }
        }
    }
}
