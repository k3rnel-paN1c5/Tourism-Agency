using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Booking;

/// <summary>
/// Represents a Data Transfer Object (DTO) for retrieving booking information.
/// Used to expose booking details to the client.
/// </summary>
public class GetBookingDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the booking.
    /// </summary>
    [Display(Name = "ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the type of booking.
    /// </summary>
    [Display(Name = "Booking Type")]
    public bool BookingType { get; set; }

    /// <summary>
    /// Gets or sets the starting date of the booking.
    /// </summary>
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the ending date of the booking.
    /// </summary>
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of the booking (e.g., Pending, Confirmed, Cancelled).
    /// </summary>
    [Display(Name = "Booking Status")]
    public BookingStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the number of passengers for the booking.
    /// </summary>
    [Display(Name = "Number Of Passengers")]
    public int NumOfPassengers { get; set; }

    /// <summary>
    /// Gets or sets the ID of the customer associated with this booking.
    /// </summary>
    [Display(Name = "Customer ID")]
    public string? CustomerId { get; set; }

    // <summary>
    /// Gets or sets the ID of the employee responsible for this booking.
    /// </summary>
    [Display(Name = "Employee Id")]
    public string? EmployeeId { get; set; } 
}
