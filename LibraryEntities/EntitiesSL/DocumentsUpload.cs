using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class DocumentsUpload
    {
        public required string id { get; set; }

        public required string docEntry { get; set; }

        public  string? description { get; set; }

        public required string user { get; set; }

        public required string name { get; set; }

        public required string attachment { get; set; }

        public  string? recipientName { get; set; }

        public  string? recipientDate { get; set; }
        public  string? recipientTime { get; set; }
    }
}
