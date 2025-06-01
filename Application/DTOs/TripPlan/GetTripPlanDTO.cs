using System.ComponentModel.DataAnnotations;
using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlanCar;

namespace Application.DTOs.TripPlan;

/// <summary>
/// Data Transfer Object for retrieving Trip Plan details.
/// </summary>
public class GetTripPlanDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the trip plan.
    /// </summary>
    [Display(Name = "Trip Plan ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Trip.
    /// </summary>
    [Display(Name = "Trip")]
    public int TripId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated Region.
    /// </summary>
    [Display(Name = "Region")]
    public int RegionId { get; set; }

    /// <summary>
    /// Gets or sets the planned start date of the trip plan.
    /// </summary>
    [Display(Name = "Starting Date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the planned end date of the trip plan.
    /// </summary>
    [Display(Name = "Ending Date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the duration date of the trip plan.
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
    /// Navigation property to the GetTrip DTO this plan belongs to.
    /// </summary>
    [Display(Name = "Trip")]
    public GetTripDTO? Trip { get; set; }

    /// <summary>
    /// Navigation property to the Region associated with this trip plan.
    /// </summary>
    [Display(Name = "Region")]
    public GetRegionDTO? Region { get; set; }

    /// <summary>
    /// Gets or sets the collection of cars assigned to the trip plan.
    /// </summary>
    [Display(Name = "Cars")]
    public ICollection<GetTripPlanCarDTO>? TripPlanCars { get; set; }
}
