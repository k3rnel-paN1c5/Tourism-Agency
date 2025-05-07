using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Region;

public class CreateRegionDTO
{
    [Required(ErrorMessage = "Name cannot be empty")]
    [StringLength(100)]
    public string? Name { get; set; }
}
