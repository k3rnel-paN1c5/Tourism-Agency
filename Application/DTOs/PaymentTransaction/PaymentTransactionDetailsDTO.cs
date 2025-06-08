using Domain.Enums;

namespace Application.DTOs.PaymentTransaction
{
    /// <summary>
    /// Data Transfer Object for detailed payment transaction information.
    /// </summary>
    public class PaymentTransactionDetailsDTO
    {
        /// <summary>
        /// Unique identifier for the payment transaction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The type of transaction.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date and time when the transaction occurred.
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// The ID of the payment this transaction belongs to.
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// The ID of the payment method used.
        /// </summary>
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Name of the payment method used.
        /// </summary>
        public string? PaymentMethodName { get; set; }

        /// <summary>
        /// Icon of the payment method used.
        /// </summary>
        public string? PaymentMethodIcon { get; set; }

        /// <summary>
        /// External transaction reference.
        /// </summary>
        // public string? TransactionReference { get; set; }

        /// <summary>
        /// Notes about the transaction.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Booking ID associated with the payment.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Payment status.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }
    }
}
