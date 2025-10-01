using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class PaymentOrderStateResponse
    {
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? OrderNumber { get; set; }
        public int OrderStatus { get; set; }
        public int ActionCode { get; set; }
        public string? ActionCodeDescription { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public long Date { get; set; }
        public string? OrderDescription { get; set; }
        public string? Ip { get; set; }
        public List<NameValueItem>? Attributes { get; set; }
        public CardAuthInfo? CardAuthInfo { get; set; }
        public PaymentAmountInfo? PaymentAmountInfo { get; set; }
        public BankInfo? BankInfo { get; set; }
        public PayerData? PayerData { get; set; }

    }
    public class NameValueItem
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
    public class CardAuthInfo
    {
        public string? MaskedPan { get; set; }
        public string? Expiration { get; set; }
        public string? CardholderName { get; set; }
        public string? PaymentSystem { get; set; }
        public string? Pan { get; set; }
    }
    public class PaymentAmountInfo
    {
        public string? PaymentState { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal DepositedAmount { get; set; }
        public decimal RefundedAmount { get; set; }
        public decimal FeeAmount { get; set; }
    }
    public class BankInfo
    {
        public string? BankName { get; set; }
        public string? BankCountryCode { get; set; }
        public string? BankCountryName { get; set; }
    }
    public class PayerData
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }



}
