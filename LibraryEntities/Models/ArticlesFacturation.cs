using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class ArticlesFacturation
    {
        public string CardCode { get; set; }
        public string? DocDate { get; set; }
        public string? DocDueDate { get; set; }
        public int? BPL_IDAssignedToInvoice { get; set; }
        public string? ShipToStreet { get; set; }
        public string? ShipToCity { get; set; }
        public string? ShipToState { get; set; }
        public List<DocumentLineFacturationArticle>? DocumentLines { get; set; }
        public string? Comments { get; set; }
    }

    public class DocumentLineFacturationArticle
    {
        public string ItemCode { get; set; }
        public int? Quantity { get; set; }
        public decimal? DiscountPercent { get; set; }
        public string? TaxCode { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? WarehouseCode { get; set; } 
        public string? CostingCode { get; set; } 
        public string? CostingCode2 { get; set; }
        public string? CostingCode3 { get; set; }
        public string? CostingCode4 { get; set; }
        public string? CostingCode5 { get; set; }
        public int? ListNum { get; set; }

    }
}
