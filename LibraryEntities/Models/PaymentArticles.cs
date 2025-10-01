using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class PaymentForArticles
    {
       public string? CardCode { get; set; }
       public string? DocDate { get; set; }
        public double? TransferSum { get; set; }
        public string? TransferAccount { get; set; }
        public string? CounterReference { get; set; }
        public string? BPLID { get; set; }
        public string? Remarks { get; set; }
        public string? U_nroTransaccion { get; set; }
        public Invoices[] PaymentInvoices { get; set; }
    }

    public class Invoices
    {
        public int? DocEntry { get; set; }
        public double? SumApplied { get; set; }

    }
}
