using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TripBooking;

public class UpdateTripBookingDTO
{
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Booking Status")]
    public BookingStatus Status { get; set; }
    
    [Required(ErrorMessage = "{0} Is required")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} Must Be At Least 1")]
    [Display(Name = "Number of Passengers")]
    public int NumOfPassengers { get; set; }
    
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Employee ID")]
    public string? EmployeeId { get; set; } 

    [Required]
    [DefaultValue(value:false)]
    [Display(Name = "Include a Guide")]
    public bool WithGuide { get; set; }
}
