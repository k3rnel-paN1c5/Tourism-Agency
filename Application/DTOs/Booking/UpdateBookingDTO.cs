using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Booking;

public class UpdateBookingDTO
{

    [Required]
    public int Id { get; set; } 

    [Required]
    public bool BookingType { get; set; }
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
}
