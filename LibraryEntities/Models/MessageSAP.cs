using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class MessageSAP
    {
        public string? Subject { get; set; }
        public string? Text { get; set; }
        public List<MessageDataColumn> MessageDataColumns { get; set; }
        public List<Recipient> RecipientCollection { get; set; }

    }

    public class MessageDataColumn
    {
        public string? ColumnName { get; set; }
        public string? Link { get; set; }
        public  List<MessageDataLine> MessageDataLines { get; set; }

    }

    public class MessageDataLine
    {
        public string? Object { get; set; }
        public string? ObjectKey { get; set; }
        public string? Value { get; set; }

    }

    public class Recipient
    {
        public string? SendInternal { get; set; }
        public string? UserCode { get; set; }
    }
}
