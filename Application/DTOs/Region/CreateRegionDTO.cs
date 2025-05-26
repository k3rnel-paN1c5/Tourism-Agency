using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class CreateRegionDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100)]
    [Display(Name = "Name")]
    public string? Name { get; set; }
}
