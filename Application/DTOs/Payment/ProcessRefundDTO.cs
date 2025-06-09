namespace Application.DTOs.Payment
{
    public class ProcessRefundDTO
    {
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}