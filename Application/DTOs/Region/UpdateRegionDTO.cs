using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class UpdateRegionDTO
{
    [Required]
    public int Id { get; set; } 

    [Required]
    [StringLength(100)]
    public string? Name { get; set; }
}
