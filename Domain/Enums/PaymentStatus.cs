namespace Domain.Enums;

/// <summary>
/// Defines the possible statuses for a payment.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// The payment is pending.
    /// </summary>
    Pending,
    /// <summary>
    /// The payment has been successfully made.
    /// </summary>
    Paid,
    /// <summary>
    /// The payment has failed.
    /// </summary>
    Failed,
    /// <summary>
    /// The payment has been fully refunded.
    /// </summary>
    Refunded,
    /// <summary>
    /// The payment has been partially refunded.
    /// </summary>
    PartiallyPaid
}