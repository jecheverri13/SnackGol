using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Attachments
    {
        [JsonProperty("Attachments2_Lines")]
        public required List<Attachments2_Lines> Attachments2_Lines { get; set; }

    }


    public class Attachments2_Lines
    {
        [JsonProperty("SourcePath")]
        public string? srcPath { get; set; }


        [JsonProperty("FreeText")]
        public string? freeText { get; set; }


        [JsonProperty("FileName")]
        public string? FileName { get; set; }


        [JsonProperty("FileExtension")]
        public string? FileExtension { get; set; }
    }
}
