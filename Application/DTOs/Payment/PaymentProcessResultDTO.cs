namespace Application.DTOs.Payment
{
    public class PaymentProcessResultDTO
    {
        public ReturnPaymentDTO Payment { get; set; }
        public ReturnPaymentTransactionDTO Transaction { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}