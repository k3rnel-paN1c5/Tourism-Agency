using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a single transaction within a payment.
/// </summary>
public partial class PaymentTransaction
{
    /// <summary>
    /// Unique identifier for the payment transaction.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Type of the transaction (e.g., Credit, Debit).
    /// </summary>
    [Required]
    [Column("type")]
    [EnumDataType(typeof(TransactionType))] // Assuming TransactionType enum exists
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Amount of money involved in this transaction.
    /// </summary>
    [Required]
    [Column("amount", TypeName = "decimal(16,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Date and time when the transaction occurred.
    /// </summary>
    [Required]
    [Column("transactionDate", TypeName = "datetime2(7)")]
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Foreign key for the associated payment.
    /// </summary>
    [Column("paymentId")]
    [ForeignKey("Payment")]
    public int PaymentId { get; set; }

    /// <summary>
    /// Foreign key for the payment method used in this transaction.
    /// </summary>
    [Column("paymentMethodId")]
    [ForeignKey("PaymentMethod")]
    public int PaymentMethodId { get; set; }


    // Navigation Proporties

    /// <summary>
    /// Navigation property to the Payment this transaction belongs to.
    /// </summary>
    public Payment? Payment { get; set; }

    /// <summary>
    /// Navigation property to the PaymentMethod used for this transaction.
    /// </summary>
    public PaymentMethod? PaymentMethod { get; set; }
}

