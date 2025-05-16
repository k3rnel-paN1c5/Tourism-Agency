using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class CreateRegionDTO
{
    [Required(ErrorMessage = "Region Name is required")]
    [StringLength(100)]
    public string? Name { get; set; }
}
