namespace Application.DTOs.Payment
{
    public class PaymentProcessResultDTO
    {
        public ReturnPaymentDTO Payment { get; set; } = null!;
        public ReturnPaymentTransactionDTO Transaction { get; set; } = null!;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}