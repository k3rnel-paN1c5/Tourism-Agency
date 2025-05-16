using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class UpdateRegionDTO
{
    [Required(ErrorMessage = "Specify Which Region To Edit")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "New Region Name Cannot Be Empty")]
    [StringLength(100)]
    public string? Name { get; set; }
}
