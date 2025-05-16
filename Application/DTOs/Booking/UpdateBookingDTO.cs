using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Booking;

public class UpdateBookingDTO
{

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "ID")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Sarting Date")]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Status")]
    public BookingStatus Status { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Number Of Passengers")]
    public int NumOfPassengers { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Employee ID")]
    public string? EmployeeId { get; set; } 
}
