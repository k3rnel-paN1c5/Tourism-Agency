using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a payment record for a booking.
/// </summary>
public partial class Payment
{
    /// <summary>
    /// Unique identifier for the payment.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key for the associated booking.
    /// </summary>
    [Required]
    [Column("bookingId")]
    [ForeignKey("Booking")]
    public int BookingId { get; set; }

    /// <summary>
    /// Current status of the payment (e.g., Pending, Completed, Refunded).
    /// </summary>
    [Required]
    [Column("status")]
    [EnumDataType(typeof(PaymentStatus))] // Assuming PaymentStatus enum exists
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Total amount due for the payment.
    /// </summary>
    [Required]
    [Column("amountDue", TypeName = "decimal(16,2)")]
    public decimal AmountDue { get; set; }

    /// <summary>
    /// Amount that has been paid so far.
    /// </summary>
    [Required]
    [Column("amountPaid", TypeName = "decimal(16,2)")]
    public decimal AmountPaid { get; set; }

    /// <summary>
    /// Date and time when the payment was made.
    /// </summary>
    [Required]
    [Column("paymentDate", TypeName = "datetime2(7)")]
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Any additional notes related to the payment.
    /// </summary>
    [Column("notes", TypeName = "nvarchar(200)")]
    public string? Notes { get; set; }

    // Navigation Properties

    /// <summary>
    /// Collection of payment transactions associated with this payment.
    /// </summary>
    public virtual ICollection<PaymentTransaction>? Transactions { get; set; }

    /// <summary>
    /// Navigation property to the Booking associated with this payment.
    /// </summary>
    public Booking? Booking { get; set; }
}
