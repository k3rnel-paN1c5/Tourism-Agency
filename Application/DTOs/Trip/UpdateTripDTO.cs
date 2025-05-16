using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class UpdateTripDTO
{
    [Required(ErrorMessage = "Specify Which Trip To Edit")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Slug is required.")]
    [StringLength(100, ErrorMessage = "Slug cannot exceed 100 characters.")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "Availability status is required.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Privacy status is required.")]
    public bool IsPrivate { get; set; }
}
