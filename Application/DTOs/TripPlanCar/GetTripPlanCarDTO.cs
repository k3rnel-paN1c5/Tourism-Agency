using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripPlanCar;

/// <summary>
/// Data Transfer Object for retrieving Trip Plan Car details.
/// </summary>
public class GetTripPlanCarDTO
{

    /// <summary>
    /// Gets or sets the unique identifier for the trip plan car entry.
    /// </summary>
    [Display(Name = "ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated Trip Plan.
    /// </summary>
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated Car.
    /// </summary>
    [Display(Name = "Car")]
    public int CarId { get; set; }

    /// <summary>
    /// Gets or sets the price of a seat in this car for this specific trip plan.
    /// </summary>
    [Display(Name = "Price")]
    public decimal Price { get; set; }
}
