using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class CreateRegionDTO
{
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }
}
