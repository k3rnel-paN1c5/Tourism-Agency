using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.PaymentTransaction
{
    /// <summary>
    /// Data Transfer Object for creating a new payment transaction.
    /// </summary>
    public class CreatePaymentTransactionDTO
    {
        /// <summary>
        /// The type of transaction (e.g., Payment, Refund).
        /// </summary>
        [Required(ErrorMessage = "Transaction type is required")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Invalid transaction type")]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The ID of the payment this transaction belongs to.
        /// </summary>
        [Required(ErrorMessage = "Payment ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment ID must be a positive number")]
        public int PaymentId { get; set; }

        /// <summary>
        /// The ID of the payment method used for this transaction.
        /// </summary>
        [Required(ErrorMessage = "Payment method ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment method ID must be a positive number")]
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Optional external transaction reference (e.g., from payment gateway).
        /// </summary>
        // [StringLength(100, ErrorMessage = "Transaction reference cannot exceed 100 characters")]
        // public string? TransactionReference { get; set; }

        /// <summary>
        /// Optional notes about the transaction.
        /// </summary>
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}