using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripBooking;

public class CreateTripBookingDTO
{
    [Required]
    public int TripPlanId { get; set; }

    [Required]
    public bool WithGuide { get; set; }
}
