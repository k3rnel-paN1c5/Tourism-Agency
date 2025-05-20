using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripPlanCar;

namespace Application.DTOs.TripPlan;

public class CreateTripPlanDTO
{
    [Required(ErrorMessage = "This Field Is Required")]
    public int TripId {get; set;}

    [Required(ErrorMessage = "This Field Is Required")]
    public int RegionId {get; set;}

    [Required(ErrorMessage = "This Field Is Required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "This Field Is Required")]
    public DateTime EndDate { get; set; }
    public TimeSpan Duration { get; set; } 
    public string? IncludedServices { get; set; }
    public string? Stops { get; set; } 
    public string? MealsPlan { get; set; }
    public string? HotelStays { get; set; } 
    
    public ICollection<CreateTripPlanCarDTO>? TripPlanCars {get;set;}

}
