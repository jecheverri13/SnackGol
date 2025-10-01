using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class FuneralNotification
    {
        [JsonProperty("U_NumID")]
        public string? docEntryOFS { get; set; }

        [JsonProperty("U_IdFallecido")]
        public string? deceasedId { get; set; }

        [JsonProperty("U_NomEncSr")]
        public string? serviceManagerName { get; set; }

        [JsonProperty("U_NumContr")]
        public string? contractNumber { get; set; }

        [JsonProperty("U_fecha")]
        public string? funeralNotificationDate { get; set; }
    }
}
