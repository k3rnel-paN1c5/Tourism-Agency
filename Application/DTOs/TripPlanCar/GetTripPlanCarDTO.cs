using System;

namespace Application.DTOs.TripPlanCar;

public class GetTripPlanCarDTO
{
    public int Id { get; set; }
    public int TripPlanId { get; set; }

    public int CarId { get; set; }

    public decimal Price { get; set; }
}
