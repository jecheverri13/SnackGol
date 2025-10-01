using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class InternalReconciliation
    {
        [JsonProperty("CardOrAccount")]
        public string? CardOrAccount { get; set; }

        [JsonProperty("InternalReconciliationOpenTransRows")]
        public List<InternalReconciliationOpenTransRows>? InternalReconciliationOpenTransRows = new List<InternalReconciliationOpenTransRows>();

    }

    public class InternalReconciliationOpenTransRows
    {
        [JsonProperty("TransId")]
        public int? TransId { get; set; }

        [JsonProperty("TransRowId")]
        public int? TransRowId { get; set; }

        [JsonProperty("ReconcileAmount")]
        public decimal? ReconcileAmount { get; set; }

        [JsonProperty("Selected")]
        public string? Selected { get; set; }

        [JsonProperty("ShortName")]
        public string? ShortName { get; set; }

    }

    
}
