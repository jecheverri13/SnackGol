using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Branch
    {
        [JsonProperty("BPLID")]
        public  string? Code { get; set; }

        [JsonProperty("BPLName")]
        public string? Name { get; set; }
    }
}
