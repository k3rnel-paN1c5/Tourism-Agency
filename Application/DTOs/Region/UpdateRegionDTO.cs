using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

/// <summary>
/// Data Transfer Object for updating an existing Region.
/// </summary>
public class UpdateRegionDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the region to be updated.
    /// This field is required.
    /// </summary>s
    [Required(ErrorMessage = "{0} is required for update.")]
    [Display(Name = "Region ID")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new name of the region.
    /// This field is required and has a maximum length of 100 characters.
    /// </summary>
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "Region name cannot exceed 100 characters.")]
    [Display(Name = "Name")]
    public string? Name { get; set; }
}
