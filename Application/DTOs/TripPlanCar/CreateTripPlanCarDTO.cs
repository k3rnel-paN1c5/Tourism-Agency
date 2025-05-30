using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripPlan;

namespace Application.DTOs.TripPlanCar;

public class CreateTripPlanCarDTO
{
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Car")]
    public int CarId { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    // public GetTripPlanDTO? TripPlanCarDTO { get; set;}
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
