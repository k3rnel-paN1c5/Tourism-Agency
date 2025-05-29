namespace Domain.Enums;

/// <summary>
/// Defines the possible statuses for a booking.
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// The booking is pending and waiting to be accepted by an employee.
    /// </summary>
    Pending,
    /// <summary>
    /// The booking has been denied.
    /// </summary>
    Denied,
    /// <summary>
    /// The booking has been confirmed but has not started yet.
    /// </summary>
    Confirmed,
    /// <summary>
    /// The booking is currently in progress.
    /// </summary>
    InProgress,
    /// <summary>
    /// The booking has been completed or ended.
    /// </summary>
    Completed,
    /// <summary>
    /// The booking has been cancelled.
    /// </summary>
    Cancelled
}