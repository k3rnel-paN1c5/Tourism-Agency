using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TripBooking;

public class CreateTripBookingDTO
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    [Required]
    public int NumOfPassengers { get; set; }
    [Required]
    public string? CustomerId { get; set; }
    public string? EmployeeId { get; set; } 
    [Required]
    public int TripPlanId { get; set; }

    [Required]
    public bool WithGuide { get; set; }

}
