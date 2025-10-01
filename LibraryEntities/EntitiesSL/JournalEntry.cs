using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class JournalEntry
    {
        [JsonProperty("ReferenceDate")]
        public string? referenceDate { get; set; }

        [JsonProperty("DueDate")]
        public string? dueDate { get; set; }

        [JsonProperty("TaxDate")]
        public string? taxDate { get; set; }

        [JsonProperty("Memo")]
        public string? comments { get; set; }


        [JsonProperty("JournalEntryLines")]
        public List<JournalEntryLines>? JournalEntryLines = new List<JournalEntryLines>();

    }

    public class JournalEntryLines
    {
        [JsonProperty("AccountCode")]
        public string? accountCode { get; set; }

        [JsonProperty("ShortName")]
        public string? shortName { get; set; }

        [JsonProperty("Debit")]
        public decimal? Debit { get; set; }

        [JsonProperty("Credit")]
        public decimal Credit { get; set; }

        [JsonProperty("LineMemo")]
        public string? lineMemo { get; set; }

        [JsonProperty("BPLID")]
        public string? branch { get; set; }
    }

    
}
