using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class IncomingPayment
    {
        [JsonProperty("CardCode")]
        public string cardCode { get; set; }

        [JsonProperty("Series")]
        public string series { get; set; }

        [JsonProperty("CashSum")]
        public string cashSum { get; set; }

        [JsonProperty("Remarks")]
        public string remarks { get; set; }

        [JsonProperty("CashAccount")]
        public string cashAccount { get; set; }

        [JsonProperty("ControlAccount")]
        public string controlAccount { get; set; }

        [JsonProperty("U_INT_DocWeb")]
        public string U_INT_DocWeb { get; set; }

        [JsonProperty("U_INT_TipoPago")]
        public string U_INT_TipoPago { get; set; }

        [JsonProperty("U_INT_Origen")]
        public string U_INT_Origen { get; set; }

        [JsonProperty("U_contrat")]
        public string U_contrat { get; set; }

        [JsonProperty("U_canal")]
        public string U_canal { get; set; }

        [JsonProperty("U_contra")]
        public string U_contra { get; set; }

        [JsonProperty("TransferAccount")]
        public string transferAccount { get; set; }

        [JsonProperty("TransferSum")]
        public string transferSum { get; set; }

        [JsonProperty("TransferReference")]
        public string transferReference { get; set; }

        [JsonProperty("TransferDate")]
        public string transferDate { get; set; }

        [JsonProperty("BPLID")]
        public string bplid { get; set; }

        [JsonProperty("U_Negocio")]
        public string U_Negocio { get; set; }

        [JsonProperty("U_asesor")]
        public int? userSign { get; set; }

        [JsonProperty("U_factsPag")]
        public string? invoices { get; set; }

        [JsonProperty("PaymentInvoices")]
        public List<PaymentInvoices>? PaymentInvoices = new List<PaymentInvoices>();

        [JsonProperty("PaymentCreditCards")]
        public List<PaymentCreditCards>? PaymentCreditCards = new List<PaymentCreditCards>();
    }

    public class PaymentInvoices
    {
        [JsonProperty("DocEntry")]
        public string? DocEntry { get; set; }

        [JsonProperty("SumApplied")]
        public int? SumApplied { get; set; }

        [JsonProperty("DocLine")]
        public int? DocLine { get; set; }

        [JsonProperty("InvoiceType")]
        public string? InvoiceType { get; set; }
    }

    public class PaymentCreditCards
    {
        [JsonProperty("CreditCard")]
        public string? CreditCard { get; set; }

        [JsonProperty("CreditCardNumber")]
        public string? CreditCardNumber { get; set; }

        [JsonProperty("CreditSum")]
        public string? CreditSum { get; set; }

        [JsonProperty("CardValidUntil")]
        public string? CardValidUntil { get; set; }
        
        [JsonProperty("VoucherNum")]
        public string? VoucherNum { get; set; }
        
        [JsonProperty("CreditAcct")]
        public string? CreditAcct { get; set; }
    }
}
