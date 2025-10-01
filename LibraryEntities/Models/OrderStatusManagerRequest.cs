namespace LibraryEntities.Models
{
    public class OrderStatusManagerRequest
    {
        public bool success { get; set; }
        public string? orderNumber { get; set; }
        public string? transactionId { get; set; }
        public StatusManagerRequest statusManager { get; set; }

    }

    public class StatusManagerRequest
    {
        public int authorizedAmount { get; set; }
        public string? note { get; set; }
        public string? payId { get; set; }
    }
}
