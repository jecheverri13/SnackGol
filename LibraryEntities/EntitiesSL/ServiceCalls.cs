using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class ServiceCalls
    {

        [JsonProperty("CustomerCode")]
        public string? document { get; set; }

        [JsonProperty("Description")]
        public string? description { get; set; }


        [JsonProperty("U_CSS_NombrePerPQR")]
        public string? name { get; set; }


        [JsonProperty("U_CSS_TelePerPQR")]
        public string? phone { get; set; }

        [JsonProperty("U_CSS_Correo")]
        public string? email { get; set; }

        [JsonProperty("CallType")]
        public int? type { get; set; }

        [JsonProperty("ItemCode")]
        public string? request { get; set; }

        [JsonProperty("U_CSS_Proceso")]
        public string? process { get; set; }

        [JsonProperty("U_CSS_SubProceso")]
        public string? subProcess { get; set; }

        [JsonProperty("U_CSS_SubProceso")]
        public int? assigned { get; set; }

    }
}
