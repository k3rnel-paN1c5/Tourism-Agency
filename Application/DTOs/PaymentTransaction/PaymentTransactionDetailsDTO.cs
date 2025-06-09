using Domain.Enums;

namespace Application.DTOs.PaymentTransaction
{
    public class PaymentTransactionDetailsDTO
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public int PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? PaymentMethodIcon { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
    }
}
