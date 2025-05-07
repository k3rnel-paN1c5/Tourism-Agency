using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripPlan;

public class UpdateTripPlanDTO
{
    [Required(ErrorMessage = "This Field Is Required")]
    public int Id {get; set;}
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
}
