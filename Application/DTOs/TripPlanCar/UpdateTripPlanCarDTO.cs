using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripPlanCar;


/// <summary>
/// Data Transfer Object for updating an existing Trip Plan Car entry.
/// </summary>
public class UpdateTripPlanCarDTO
{

    /// <summary>
    /// Gets or sets the unique identifier of the trip plan car entry to be updated.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "ID")]
    public int Id { get; set; }

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
    /// </summary
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Car")]
    public int CarId { get; set; }

    /// <summary>
    /// Gets or sets the updated price of a seat in this car for this specific trip plan.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

}
