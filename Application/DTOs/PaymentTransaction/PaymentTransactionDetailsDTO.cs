using Domain.Enums;

namespace Application.DTOs.PaymentTransaction
{
    public class PaymentTransactionDetailsDTO
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public int TransactionMethodId { get; set; }
        public string? TransactionMethodName { get; set; }
        public string? TransactionMethodIcon { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
    }
}
