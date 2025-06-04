using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TripBooking;

/// <summary>
/// Data Transfer Object for retrieving Trip Booking details.
/// </summary>
public class GetTripBookingDTO
{
    /// <summary>
    /// Data Transfer Object for retrieving Trip Booking details.
    /// </summary>
    [Display(Name = "ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the planned start date of the trip booking.
    /// </summary>
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the planned end date of the trip booking.
    /// </summary>
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of the trip booking.
    /// </summary>
    [Display(Name = "Booking Status")]
    public BookingStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the number of passengers for the trip booking.
    /// </summary>
    [Display(Name = "Number of Passengers")]
    public int NumOfPassengers { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Customer.
    /// </summary>
    [Display(Name = "Customer ID")]
    public string? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Employee.
    /// </summary>
    [Display(Name = "Employee ID")]
    public string? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Trip Plan.
    /// </summary>
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a guide is included in the trip.
    /// </summary>
    [Display(Name = "Includes a Guide")]
    public bool WithGuide { get; set; }
}
