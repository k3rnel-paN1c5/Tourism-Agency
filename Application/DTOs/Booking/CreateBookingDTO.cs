using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Booking;

public class CreateBookingDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Booking Type")]
    public bool BookingType { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Sarting Date")]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Number Of Passengers")]
    public int NumOfPassengers { get; set; }

}
