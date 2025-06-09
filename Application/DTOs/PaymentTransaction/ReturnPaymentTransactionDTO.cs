using Domain.Enums;

namespace Application.DTOs.PaymentTransaction
{
    public class ReturnPaymentTransactionDTO
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
    }
}
