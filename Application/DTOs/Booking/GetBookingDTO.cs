using System;
using Domain.Enums;

namespace Application.DTOs.Booking;

public class GetBookingDTO
{
    public int Id { get; set; }
    public bool BookingType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookingStatus Status { get; set; }
    public int NumOfPassengers { get; set; }
    public string? CustomerId { get; set; }
    public string? EmployeeId { get; set; } 
}
