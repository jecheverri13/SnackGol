using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class ResolutionResponse
    {
        public string IdSerie { get; set; }
        public string? Prefix { get; set; }
        public string? ResolutionNumber { get; set; }
        public long InitConsecutive { get; set; }
        public long CurrentConsecutive { get; set; }
        public long FinalConsecutive { get; set; }
        public string? InitDate { get; set; }
        public string? FinalDate { get; set; }
        public bool Status { get; set; }
    }
}
