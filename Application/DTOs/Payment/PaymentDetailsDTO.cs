using Application.DTOs.PaymentTransaction;
using Domain.Enums;

namespace Application.DTOs.Payment
{
    public class PaymentDetailsDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }
        
        // Transaction history
        public List<ReturnPaymentTransactionDTO> Transactions { get; set; } = new();
        
        // Calculated properties
        public decimal RemainingBalance => AmountDue - AmountPaid;
        public bool IsFullyPaid => AmountPaid >= AmountDue;
        public bool HasTransactions => Transactions.Any();
    }
}