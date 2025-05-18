using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TripBooking;

public class UpdateTripBookingDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public BookingStatus Status { get; set; }
    [Required]
    public int NumOfPassengers { get; set; }
    [Required]
    public string? CustomerId { get; set; }
    public string? EmployeeId { get; set; } 

    [Required]
    public bool WithGuide { get; set; }
}
