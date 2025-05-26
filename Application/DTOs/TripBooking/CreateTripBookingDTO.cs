using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripBooking;

public class CreateTripBookingDTO
{
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "{0} Is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "{0} Is required")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} Must Be At Least 1")]
    [Display(Name = "Number of Passengers")]
    public int NumOfPassengers { get; set; }

    [Required]
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    [Required]
    [DefaultValue(value:false)]
    [Display(Name = "Includes a Guide")]
    public bool WithGuide { get; set; }
}
