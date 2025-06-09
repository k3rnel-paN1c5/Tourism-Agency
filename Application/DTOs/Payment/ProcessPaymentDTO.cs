namespace Application.DTOs.Payment
{
    public class ProcessPaymentDTO
    {
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        //public string? TransactionReference { get; set; }
        public string? Notes { get; set; }
    }
}