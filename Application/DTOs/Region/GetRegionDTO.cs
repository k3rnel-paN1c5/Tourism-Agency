using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

/// <summary>
/// Data Transfer Object for retrieving Region details.
/// </summary>
public class GetRegionDTO
{
    // <summary>
    /// Gets or sets the unique identifier of the region.
    /// </summary>
    [Display(Name = "Region ID")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the region.
    /// </summary>
    [Display(Name = "Name")]
    public string? Name { get; set; }
}
