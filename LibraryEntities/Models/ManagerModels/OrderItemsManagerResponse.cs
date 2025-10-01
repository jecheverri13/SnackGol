using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models.ManagerModels
{
        public class OrderItemsManagerResponse
    {
            public string CheckoutPrefix { get; set; }
            public Receipt Receipt { get; set; }
            public object? Customer { get; set; }
        }

        public class Receipt
        {
            public object? ShipmentTrackingInformation { get; set; }
            public string FollowUpReceiptId { get; set; }
            public object? Metadata { get; set; }
            public int ServerVersion { get; set; }
            public int ConcessionTableIndex { get; set; }
            public List<object> AdditionalFields { get; set; }
            public string DiscountPurposeCode { get; set; }
            public double DiscountAmount { get; set; }
            public object? CustomerData { get; set; }
            public double TotalGrossAmount { get; set; }
            public double ServiceChargeNetAmount { get; set; }
            public string OriginalReceiptId { get; set; }
            public List<OrderBundle> OrderBundles { get; set; }
            public List<object> Coupons { get; set; }
            public object? PrintLogo { get; set; }
            public double RoundingValue { get; set; }
            public object? LoyaltyAccountInformation { get; set; }
            public string Id { get; set; }
            public string CancellationStatus { get; set; }
            public double PaymentNetAmountWithoutReceiptDiscount { get; set; }
            public double ServiceChargeGrossAmount { get; set; }
            public double DiscountNetAmount { get; set; }
            public bool CashDeskClosingCashOut { get; set; }
            public bool ContainsCancellationSalesItem { get; set; }
            public double ServiceChargeTaxAmount { get; set; }
            public List<SalesItem> SalesItems { get; set; }
            public double NotRoundedPaymentGrossAmount { get; set; }
            public int CentralServerPostingStatus { get; set; }
            public double FeeTaxAmount { get; set; }
            public string FormattedNameCashierStyle { get; set; }
            public bool IsCashDeskClosing { get; set; }
            public string SoftwareVersion { get; set; }
            public string Status { get; set; }
            public string BupaExternalId { get; set; }
            public object? Signature { get; set; }
            public string PickupId { get; set; }
            public bool Confirmed { get; set; }
            public double DiscountPercentage { get; set; }
            public string AddressMode { get; set; }
            public long BusinessTransactionDate { get; set; }
            public List<object> CouponAssignments { get; set; }
            public string PosSystemId { get; set; }
            public string Currency { get; set; }
            public string LastExternalId { get; set; }
            public string ModifiedBy { get; set; }
            public bool RoundingUsed { get; set; }
            public bool ReverseReceipt { get; set; }
            public string CashDeskClosingID { get; set; }
            public string ManuallyPostedBy { get; set; }
            public string PriceListId { get; set; }
            public double FeeNetAmount { get; set; }
            public List<PaymentItem> PaymentItems { get; set; }
            public string TaxExemptionReasonCode { get; set; }
            public int CountPrintouts { get; set; }
            public object? CalculationMetaData { get; set; }
            public double PaymentTaxAmount { get; set; }
            public object? SalesPerson { get; set; }
            public int CustomerCount { get; set; }
            public object? LoyaltyAccount { get; set; }
            public List<object> AlternateIds { get; set; }
            public bool ContainsOnlyCancellationSalesItem { get; set; }
            public long ModifiedAt { get; set; }
            public string ExternalID { get; set; }
            public double PaymentGrossAmount { get; set; }
            public object? TaxationMethod { get; set; }
            public List<TaxItem> TaxItems { get; set; }
            public long CreatedAt { get; set; }
            public double DiscountablePaymentGrossAmount { get; set; }
            public string TaxExemptionCertificateNumber { get; set; }
            public string GlobalUniqueId { get; set; }
            public double PaymentGrossAmountWithoutReceiptDiscount { get; set; }
            public double DiscountablePaymentNetAmount { get; set; }
            public double PaymentNetAmountWithoutVoucher { get; set; }
            public string SubTypeCode { get; set; }
            public string PosGroupId { get; set; }
            public string CompanyId { get; set; }
            public bool Immutable { get; set; }
            public bool PercentageDiscount { get; set; }
            public double PaymentNetAmount { get; set; }
            public bool ManuallyPosted { get; set; }
            public double TotalTaxAmount { get; set; }
            public string TaxExemptionReasonDescription { get; set; }
            public object? ConcessionTable { get; set; }
            public string BusinessTransactionTimeZone { get; set; }
            public string UiMode { get; set; }
            public List<object> ChargeElements { get; set; }
            public string RoundingLevel { get; set; }
            public bool CreatedByConflict { get; set; }
            public List<object> LoyaltyPayments { get; set; }
            public bool VoucherInPaymentItems { get; set; }
            public bool RemoteCalculated { get; set; }
            public int ErpPostingStatus { get; set; }
            public object? DebtorItem { get; set; }
            public object? Owner { get; set; }
            public double TotalNetAmount { get; set; }
            public int LoyaltyAccountTotalPoints { get; set; }
            public string TypeCode { get; set; }
            public double FeeGrossAmount { get; set; }
            public bool PriceListManuallyChanged { get; set; }
            public object? ShipmentInformation { get; set; }
            public string CreatedBy { get; set; }
            public bool Productive { get; set; }
            public object? CashDeskClosing { get; set; }
            public int SalesPersonNameStyle { get; set; }
            public string SiteId { get; set; }
            public string Comment { get; set; }
            public double PaymentGrossAmountWithoutVoucher { get; set; }
            public string ReceiptID { get; set; }
            public object? Customer { get; set; }
        }

        public class OrderBundle
        {
            public object? Owner { get; set; }
            public List<SalesItemAssociation> SalesItemAssociations { get; set; }
            public List<object> AlternateIds { get; set; }
            public List<object> AdditionalFields { get; set; }
            public long ModifiedAt { get; set; }
            public string Description { get; set; }
            public long CreatedAt { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedBy { get; set; }
            public string Id { get; set; }
            public string ReceiptId { get; set; }
            public string Status { get; set; }
        }

        public class SalesItemAssociation
        {
            public long CreatedAt { get; set; }
            public List<object> AlternateIds { get; set; }
            public int Quantity { get; set; }
            public List<object> AdditionalFields { get; set; }
            public string CreatedBy { get; set; }
            public long ModifiedAt { get; set; }
            public string OrderBundleId { get; set; }
            public string ModifiedBy { get; set; }
            public object? OrderBundleStatus { get; set; }
            public string SalesItemId { get; set; }
        }

        public class SalesItem
        {
            public object? Metadata { get; set; }
            public List<object> AdditionalFields { get; set; }
            public string DiscountPurposeCode { get; set; }
            public string FinancialTransactionType { get; set; }
            public double DiscountAmount { get; set; }
            public object? ErpSalesBusinessObjectType { get; set; }
            public int LoyaltyPoints { get; set; }
            public double ServiceChargeNetAmount { get; set; }
            public string PromotionId { get; set; }
            public string DiscountPurposeCodeFromReceipt { get; set; }
            public string ErpSalesBusinessObjectSeries { get; set; }
            public string Id { get; set; }
            public string ErpSalesObjectProjectId { get; set; }
            public double PaymentNetAmountWithoutReceiptDiscount { get; set; }
            public bool DeliveredNow { get; set; }
            public double ServiceChargeGrossAmount { get; set; }
            public string RecursiveParentReceiptKey { get; set; }
            public bool DeliveredQuantityManuallyChanged { get; set; }
            public double DiscountNetAmount { get; set; }
            public double ServiceChargeTaxAmount { get; set; }
            public bool AutomaticBatchOrSerialNumberDeterminationEnabled { get; set; }
            public List<object> OrderBundleAssociations { get; set; }
            public string QuantityTypeCodeName { get; set; }
            public double NotRoundedPaymentGrossAmount { get; set; }
            public double FeeTaxAmount { get; set; }
            public string TaxCountryCode { get; set; }
            public bool Deposit { get; set; }
            public string ErpSalesBusinessObjectPosition { get; set; }
            public string Status { get; set; }
            public string StatusCode { get; set; }
            public string OriginalGenericArticleId { get; set; }
            public bool TaxRateTypeCodeChanged { get; set; }
            public string StockArea { get; set; }
            public double DiscountPercentage { get; set; }
            public double DiscountAmountFromReceipt { get; set; }
            public string MiscellaneousJson { get; set; }
            public string ManagedByNumber { get; set; }
            public string ModifiedBy { get; set; }
            public object? DeliveryDate { get; set; }
            public string TaxHintCode { get; set; }
            public double UnitNetAmount { get; set; }
            public bool InSetEditMode { get; set; }
            public object? ReferenceSalesItem { get; set; }
            public string PriceListId { get; set; }
            public bool ProjectPostingAllowedAccordingToGLAliasCode { get; set; }
            public double FeeNetAmount { get; set; }
            public string TaxExemptionReasonCode { get; set; }
            public string GeneralLedgerAccountAliasCode { get; set; }
            public string Series { get; set; }
            public double DiscountNetAmountFromReceipt { get; set; }
            public double PaymentTaxAmount { get; set; }
            public object? SalesPerson { get; set; }
            public List<object> AlternateIds { get; set; }
            public List<object> Notes { get; set; }
            public bool VoucherFundsTransferred { get; set; }
            public long ModifiedAt { get; set; }
            public object? Voucher { get; set; }
            public string ExternalID { get; set; }
            public bool CashDiscountDeductibleIndicator { get; set; }
            public string ErpSalesBusinessObjectId { get; set; }
            public double PaymentGrossAmount { get; set; }
            public List<object> TaxItems { get; set; }
            public long CreatedAt { get; set; }
            public bool ItemDiscountAlreadyFetched { get; set; }
            public double DeliveredQuantity { get; set; }
            public string ManagedBy { get; set; }
            public double SubItemBaseUnitPrice { get; set; }
            public string TaxExemptionCertificateNumber { get; set; }
            public string GlobalUniqueId { get; set; }
            public bool UnitPriceChanged { get; set; }
            public string ProductTaxationCharacteristicsCode { get; set; }
            public string CourseId { get; set; }
            public List<PriceElement> PriceElements { get; set; }
            public string QuantityTypeCode { get; set; }
            public bool SetCreatedByBestPriceFinding { get; set; }
            public double PaymentGrossAmountWithoutReceiptDiscount { get; set; }
            public double UnitTaxAmount { get; set; }
            public string CalculatedQuantityTypeCode { get; set; }
            public double GrossAmount { get; set; }
            public string TaxRateTypeCode { get; set; }
            public double TaxRate { get; set; }
            public bool Immutable { get; set; }
            public bool PercentageDiscount { get; set; }
            public List<object> ManagedByDetails { get; set; }
            public List<object> SubItems { get; set; }
            public double UnitNetAmountOrigin { get; set; }
            public double PaymentNetAmount { get; set; }
            public bool SplitForced { get; set; }
            public double TaxAmount { get; set; }
            public string UnitBaseQuantityTypeCode { get; set; }
            public string TaxExemptionReasonDescription { get; set; }
            public string Note { get; set; }
            public List<object> ChargeElements { get; set; }
            public string Description { get; set; }
            public double UnitGrossAmount { get; set; }
            public double UnitGrossAmountOrigin { get; set; }
            public double SubItemBaseQuantity { get; set; }
            public bool CancellationSalesItem { get; set; }
            public bool Discountable { get; set; }
            public string ReceiptId { get; set; }
            public double UnitBaseQuantity { get; set; }
            public double CalculatedQuantity { get; set; }
            public double Quantity { get; set; }
            public string CustomerReturnReasonCode { get; set; }
            public double NetAmount { get; set; }
            public List<object> CancellationSalesItems { get; set; }
            public string DeliveryType { get; set; }
            public string TypeCode { get; set; }
            public double FeeGrossAmount { get; set; }
            public Material Material { get; set; }
            public string CreatedBy { get; set; }
            public bool ItemDiscountChanged { get; set; }
        }

        public class PriceElement
        {
            public double Amount { get; set; }
            public bool Manually { get; set; }
            public string Ident { get; set; }
            public long ModifiedAt { get; set; }
            public string ExtIdent { get; set; }
            public string ExternalID { get; set; }
            public string Type { get; set; }
            public long CreatedAt { get; set; }
            public double BaseQuantity { get; set; }
            public string CreatedBy { get; set; }
            public double Percentage { get; set; }
            public double AmountTotal { get; set; }
            public string ModifiedBy { get; set; }
            public int Position { get; set; }
        }

        public class Material
        {
            public string ProdCatID { get; set; }
            public string ExternalID { get; set; }
            public string MaterialDescription { get; set; }
        }

        public class PaymentItem
        {
            public string Note { get; set; }
            public double RoundingAmount { get; set; }
            public List<object> TransactionDetails { get; set; }
            public object? Metadata { get; set; }
            public List<object> AlternateIds { get; set; }
            public List<object> AdditionalFields { get; set; }
            public string PaymentFormCode { get; set; }
            public long ModifiedAt { get; set; }
            public object? Voucher { get; set; }
            public string HouseBankAccountInternalID { get; set; }
            public string OriginalBusinessTransactioncurrency { get; set; }
            public double OriginalBusinessTransactionAmount { get; set; }
            public string ExternalID { get; set; }
            public string PaymentTransactionReferenceID { get; set; }
            public double ExchangeRateUsed { get; set; }
            public string CreditCardErpTypeCode { get; set; }
            public string CreditCardTypeCode { get; set; }
            public long CreatedAt { get; set; }
            public string PettyCashID { get; set; }
            public string ModifiedBy { get; set; }
            public List<object> PaymentDetails { get; set; }
            public string ReceiptId { get; set; }
            public string PayerBusinessPartnerInternalID { get; set; }
            public bool TransferredToServer { get; set; }
            public string CreditCardId { get; set; }
            public string CreditCardTypeName { get; set; }
            public long TransactionDate { get; set; }
            public string PaymentTerminalId { get; set; }
            public string AdditionalPaymentReference { get; set; }
            public string CreatedBy { get; set; }
            public double BusinessTransactionAmount { get; set; }
            public string CreditCardNumber { get; set; }
            public string BusinessTransactioncurrency { get; set; }
            public string Status { get; set; }
            public string StatusCode { get; set; }
        }

        public class TaxItem
        {
            public double TaxableAmount { get; set; }
            public List<object> AlternateIds { get; set; }
            public List<object> AdditionalFields { get; set; }
            public bool DeferredIndicator { get; set; }
            public long ModifiedAt { get; set; }
            public double NonTaxableAmount { get; set; }
            public string Description { get; set; }
            public string ExternalID { get; set; }
            public double TaxExemptAmount { get; set; }
            public double GrossAmount { get; set; }
            public string TaxRateTypeCode { get; set; }
            public string TaxJurisdictionName { get; set; }
            public string UsedExternalSystem { get; set; }
            public long CreatedAt { get; set; }
            public string TaxCountryCode { get; set; }
            public double TaxRate { get; set; }
            public string CreatedBy { get; set; }
            public double BusinessTransactionAmount { get; set; }
            public string TaxJurisdictionDistrictType { get; set; }
            public string ModifiedBy { get; set; }
            public string ProductTaxationCharacteristicsCode { get; set; }
            public string Region { get; set; }
            public string ReceiptId { get; set; }
        }


}
