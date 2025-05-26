using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlanCar;

namespace Application.DTOs.TripPlan;

public class CreateTripPlanDTO
{
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Trip")]
    public int TripId {get; set;}

    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Region")]
    public int RegionId {get; set;}

    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }
    [Display(Name = "Duration")]
    public TimeSpan Duration { get; set; } 
    [Display(Name = "Included Services")]
    public string? IncludedServices { get; set; }
    [Display(Name = "Stops")]
    public string? Stops { get; set; } 
    [Display(Name = "Meals Plan")]
    public string? MealsPlan { get; set; }
    [Display(Name = "Hotel Stays")]
    public string? HotelStays { get; set; } 
    
    // Navigation Properties
    [Display(Name = "Trip")]
    public GetTripDTO? Trip {get; set;}

    [Display(Name = "Region")]
    public GetRegionDTO? Region {get; set;}
    
    [Display(Name = "Cars")]
    public ICollection<CreateTripPlanCarDTO>? TripPlanCars {get;set;}

}
