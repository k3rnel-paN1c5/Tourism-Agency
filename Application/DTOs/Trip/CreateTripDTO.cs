using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

/// <summary>
/// Data Transfer Object for creating a new Trip.
/// </summary>
public class CreateTripDTO
{
    /// <summary>
    /// Gets or sets the name of the trip.
    /// This field is required and has a maximum length of 50 characters.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot exceed 50 characters.")]
    [Display(Name = "Trip Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the URL-friendly slug for the trip.
    /// This field has a maximum length of 100 characters. If not provided, it will be auto-generated.
    /// </summary>
    [StringLength(100, ErrorMessage = "{0} cannot exceed 100 characters.")]
    [Display(Name = "Trip Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the trip is available.
    /// Defaults to true.
    /// </summary>
    [Required]
    [DefaultValue(value: true)]
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Gets or sets the description of the trip.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the trip is private.
    /// Defaults to false.
    /// </summary>
    [Required]
    [DefaultValue(value: false)]
    [Display(Name = "Private Trip")]
    public bool IsPrivate { get; set; }

}
