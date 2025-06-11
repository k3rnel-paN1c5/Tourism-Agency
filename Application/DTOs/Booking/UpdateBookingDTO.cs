using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Booking;

/// <summary>
/// Represents a Data Transfer Object (DTO) for updating an existing booking.
/// Used to modify the details of a booking.
/// </summary>
public class UpdateBookingDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the booking to be updated.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new starting date of the booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Sarting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the new ending date of the booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the new status of the booking (e.g., Pending, Confirmed, Cancelled).
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Status")]
    public BookingStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the new number of passengers for the booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Number Of Passengers")]
    public int NumOfPassengers { get; set; }

    /// <summary>
    /// Gets or sets the ID of the employee updating or managing this booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Employee ID")]
    public string? EmployeeId { get; set; } 
}
