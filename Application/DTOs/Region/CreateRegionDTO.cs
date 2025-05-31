using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

/// <summary>
/// Data Transfer Object for creating a new Region.
/// </summary>
public class CreateRegionDTO
{
    /// <summary>
    /// Gets or sets the name of the region.
    /// This field is required and has a maximum length of 100 characters.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "Region name cannot exceed 100 characters.")]
    [Display(Name = "Name")]
    public string? Name { get; set; }
}