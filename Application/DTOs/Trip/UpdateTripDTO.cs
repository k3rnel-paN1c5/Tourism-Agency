using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

/// <summary>
/// Data Transfer Object for updating an existing Trip.
/// </summary>
public class UpdateTripDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the trip to be updated.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Trip ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new name of the trip.
    /// This field is required and has a maximum length of 50 characters.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(50, ErrorMessage = "{0} cannot exceed 50 characters.")]
    [Display(Name = "Trip Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the new URL-friendly slug for the trip.
    /// This field is required and has a maximum length of 100 characters.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(100, ErrorMessage = "{0} cannot exceed 100 characters.")]
    [Display(Name = "Trip Sug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the new availability status of the trip.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Availablity")]
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Gets or sets the new description of the trip.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "{0}  is required.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the new privacy status of the trip.
    /// This field is required.
    /// </summary>
    [Display(Name = "Private Trip")]
    [Required(ErrorMessage = "Privacy status is required.")]
    public bool IsPrivate { get; set; }
}
