using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TripBooking;

public class GetTripBookingDTO
{
    [Display(Name = "ID")]
    public int Id { get; set; }
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }
    [Display(Name = "Booking Status")]
    public BookingStatus Status { get; set; }
    [Display(Name = "Number of Passengers")]
    public int NumOfPassengers { get; set; }
    [Display(Name = "Customer ID")]
    public string? CustomerId { get; set; }
    [Display(Name = "Employee ID")]
    public string? EmployeeId { get; set; }
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }
    [Display(Name = "Includes a Guide")]
    public bool WithGuide { get; set; }
}
