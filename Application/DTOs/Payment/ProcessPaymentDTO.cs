namespace Application.DTOs.Payment
{
    public class ProcessPaymentDTO
    {
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}