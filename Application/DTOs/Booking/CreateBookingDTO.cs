using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Booking;
/// <summary>
/// Represents a Data Transfer Object (DTO) for creating a new booking.
/// Used to capture the necessary information from the client when a booking is initiated.
/// </summary>
public class CreateBookingDTO
{
    /// <summary>
    /// Gets or sets a value indicating the type of booking (e.g., true for Trip Booking, false for Car Booking).
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Trip Booking")]
    public bool IsTripBooking { get; set; }

    /// <summary>
    /// Gets or sets the starting date of the booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Sarting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the ending date of the booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the number of passengers for the booking.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Number Of Passengers")]
    public int NumOfPassengers { get; set; }

}
