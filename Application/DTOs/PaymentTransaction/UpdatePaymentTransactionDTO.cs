using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PaymentTransaction
{
    /// <summary>
    /// Data Transfer Object for updating payment transaction information.
    /// </summary>
    public class UpdatePaymentTransactionDTO
    {
        /// <summary>
        /// Unique identifier for the payment transaction.
        /// </summary>
        [Required(ErrorMessage = "Transaction ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Transaction ID must be a positive number")]
        public int Id { get; set; }

        /// <summary>
        /// External transaction reference.
        /// </summary>
        // [StringLength(100, ErrorMessage = "Transaction reference cannot exceed 100 characters")]
        // public string? TransactionReference { get; set; }

        /// <summary>
        /// Notes about the transaction.
        /// </summary>
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}