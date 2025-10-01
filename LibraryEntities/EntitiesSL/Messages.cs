using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Messages
    {
        [JsonProperty("Priority")]
        public required string priority { get; set; }

        [JsonProperty("Subject")]
        public required string subject { get; set; }

        [JsonProperty("Text")]
        public required string text { get; set; }

        [JsonProperty("RecipientCollection")]
        public List<RecipientCollection> recipientCollection = new List<RecipientCollection>();
    }

    public class RecipientCollection
    {
        [JsonProperty("UserCode")]
        public required string userCode { get; set; }

        [JsonProperty("SendInternal")]
        public required string sendInternal { get; set; }
    }
}
