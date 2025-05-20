using System;

namespace Application.DTOs.TripPlan;

public class GetTripPlanDTO
{
    public int Id {get; set;}
    public int TripId {get; set;}
    public int RegionId {get; set;}
    public DateTime StartDate { get; set; }
    public DateTime EndtDate { get; set; }
    public TimeSpan Duration { get; set; } 
    public string? IncludedServices { get; set; }
    public string? Stops { get; set; } 
    public string? MealsPlan { get; set; }
    public string? HotelStays { get; set; } 
}
