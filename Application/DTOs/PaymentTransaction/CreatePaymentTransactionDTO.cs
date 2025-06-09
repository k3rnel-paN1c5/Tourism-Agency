using Domain.Enums;

namespace Application.DTOs.PaymentTransaction
{
    public class CreatePaymentTransactionDTO
    {
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Notes { get; set; }
    }
}
