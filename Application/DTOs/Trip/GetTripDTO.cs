using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

/// <summary>
/// Data Transfer Object for retrieving Trip details.
/// </summary>
public class GetTripDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the trip.
    /// </summary>
    [Display(Name = "Trip ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the trip.
    /// </summary>
    [Display(Name = "Trip Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the URL-friendly slug for the trip.
    /// </summary>
    [Display(Name = "Trip Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the description of the trip.
    /// </summary>
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the trip is available.
    /// </summary>
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the trip is private.
    /// </summary>
    [Display(Name = "Private Trip")]
    public bool IsPrivate { get; set; }
}
