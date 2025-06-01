using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripPlan;

namespace Application.DTOs.TripPlanCar;


/// <summary>
/// Data Transfer Object for creating a Trip Plan Car entry from the Trip Plan service.
/// The use of this DTO avoid having to deal with the dates twice.
/// </summary>
public class CreateTripPlanCarFromTripPlanDTO
{

    /// <summary>
    /// Gets or sets the ID of the associated Trip Plan.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated Car.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Car")]
    public int CarId { get; set; }

    /// <summary>
    /// Gets or sets the price of the seat in this car for this specific trip plan.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Navigation property to derive the start and end date.
    /// </summary>
    public GetTripPlanDTO? TripPlan { get; set; } = null;
}
