using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlanCar;

namespace Application.DTOs.TripPlan;

/// <summary>
/// Data Transfer Object for creating a new Trip Plan.
/// </summary>
public class CreateTripPlanDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the associated Trip.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Trip")]
    public int TripId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Region.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Region")]
    public int RegionId { get; set; }

    /// <summary>
    /// Gets or sets the planned start date of the trip plan.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the planned end date of the trip plan.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the planned duration of the trip plan.
    /// </summary>
    [Display(Name = "Duration")]
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets the included services in the trip plan.
    /// </summary>
    [Display(Name = "Included Services")]
    public string? IncludedServices { get; set; }

    /// <summary>
    /// Gets or sets the planned stop points of the trip plan.
    /// </summary>
    [Display(Name = "Stops")]
    public string? Stops { get; set; }

    /// <summary>
    /// Gets or sets the meals plan of the trip plan.
    /// </summary>
    [Display(Name = "Meals Plan")]
    public string? MealsPlan { get; set; }

    /// <summary>
    /// Gets or sets the planned hotel stays of the trip plan.
    /// </summary>
    [Display(Name = "Hotel Stays")]
    public string? HotelStays { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the collection of cars with their prices assigned to the trip plan.
    /// </summary>
    [Display(Name = "Cars")]
    public ICollection<CreateTripPlanCarFromTripPlanDTO>? TripPlanCars { get; set; }

}
