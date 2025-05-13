using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripBooking;

public class UpdateTripBookingDTO
{
    [Required]
    public int BookingId { get; set; }

    [Required]
    public bool WithGuide { get; set; }
}
