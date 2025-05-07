using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class UpdateRegionDTO
{
    [Required(ErrorMessage = "Specify which region to edit")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "New region name cannot be empty")]
    [StringLength(100)]
    public string? Name { get; set; }
}
