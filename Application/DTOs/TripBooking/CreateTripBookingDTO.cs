using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripBooking;

/// <summary>
/// Data Transfer Object for creating a new Trip Booking.
/// </summary>
public class CreateTripBookingDTO
{
    /// <summary>
    /// Gets or sets the planned start date of the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the planned end date of the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the number of passengers for the trip booking.
    /// </summary>
    [Required(ErrorMessage = "{0} Is required")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} Must Be At Least 1")]
    [Display(Name = "Number of Passengers")]
    public int NumOfPassengers { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Trip Plan.
    /// </summary>
    [Required]
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a guide is included in the trip.
    /// </summary>
    [Required]
    [DefaultValue(value: false)]
    [Display(Name = "Includes a Guide")]
    public bool WithGuide { get; set; }
}
