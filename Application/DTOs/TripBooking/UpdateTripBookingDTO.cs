using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TripBooking;

/// <summary>
/// Data Transfer Object for updating an existing Trip Booking.
/// </ summary>
public class UpdateTripBookingDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the trip booking to update.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated planned start date of the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the updated planned end date of the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the updated status of the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Booking Status")]
    public BookingStatus Status { get; set; }   


    /// <summary>
    /// Gets or sets the updated number of passengers for the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} Must Be At Least 1")]
    [Display(Name = "Number of Passengers")]
    public int NumOfPassengers { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the Employee managing this booking.
    /// </summary> 
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Employee ID")]
    public string? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a guide is included in the trip.
    /// </summary>
    [Required]
    [DefaultValue(value: false)]
    [Display(Name = "Include a Guide")]
    public bool WithGuide { get; set; }
}
