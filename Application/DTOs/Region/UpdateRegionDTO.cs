using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class UpdateRegionDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Region ID")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100)]
    [Display(Name = "Name")]
    public string? Name { get; set; }
}
