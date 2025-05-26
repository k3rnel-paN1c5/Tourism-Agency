using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class GetRegionDTO
{
    [Display(Name = "Region ID")]
    public int Id { get; set; }
    [Display(Name = "Name")]
    public string? Name { get; set; }
}
