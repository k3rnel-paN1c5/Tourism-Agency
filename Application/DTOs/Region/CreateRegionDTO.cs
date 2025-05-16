using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class CreateRegionDTO
{
    [Required(ErrorMessage = "Region Name Cannot Be Empty")]
    [StringLength(100)]
    public string? Name { get; set; }
}
