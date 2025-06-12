using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlanCar;

namespace Application.DTOs.TripPlan;


/// <summary>
/// Data Transfer Object for updating an existing Trip Plan.
/// </summary>
public class UpdateTripPlanDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the Trip Plan.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "ID")]
    public int Id { get; set; }

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
    /// Gets or sets the planned duration date of the trip plan.
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

    /// <summary>
    /// Gets or sets the updated price of a seat in this car for this specific trip plan.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

}
