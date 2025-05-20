using System;

namespace Application.DTOs.TripBooking;

public class GetTripBookingDTO
{
    public int BookingId { get; set; }
    public int TripPlanId { get; set; }
    public bool WithGuide { get; set; }
}
