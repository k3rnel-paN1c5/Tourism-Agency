using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class CreateTripDTO
{
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    [Required]
    [StringLength(100)]
    public string? Slug { get; set; }

    [Required]
    public bool IsAvailable { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public bool IsPrivate { get; set; }

}
