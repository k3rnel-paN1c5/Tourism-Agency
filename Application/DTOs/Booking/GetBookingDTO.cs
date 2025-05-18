using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Booking;

public class GetBookingDTO
{
    [Display(Name = "ID")]
    public int Id { get; set; }
    
    [Display(Name = "Booking Type")]
    public bool BookingType { get; set; }

    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Booking Status")]
    public BookingStatus Status { get; set; }

    [Display(Name = "Number Of Passengers")]
    public int NumOfPassengers { get; set; }

    [Display(Name = "Customer ID")]
    public string? CustomerId { get; set; }
    
    [Display(Name = "Employee Id")]
    public string? EmployeeId { get; set; } 
}
