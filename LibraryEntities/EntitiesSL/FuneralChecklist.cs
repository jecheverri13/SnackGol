using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class FuneralChecklist
    {
        [JsonProperty("U_OK1_SF_NSOL")]
        public string? docEntryOFS { get; set; }
    }
}
