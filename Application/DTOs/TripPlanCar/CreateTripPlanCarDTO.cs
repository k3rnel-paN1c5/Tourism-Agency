using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripPlan;

namespace Application.DTOs.TripPlanCar;


/// <summary>
/// Data Transfer Object for creating a new Trip Plan Car entry.
/// </summary>
public class CreateTripPlanCarDTO
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
    /// Gets or sets the start date for the car booking within the trip plan.
    /// This is typically derived from the parent Trip Plan's start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date for the car booking within the trip plan.
    /// This is typically derived from the parent Trip Plan's end date.
    /// </summary>
    public DateTime EndDate { get; set; }
}
