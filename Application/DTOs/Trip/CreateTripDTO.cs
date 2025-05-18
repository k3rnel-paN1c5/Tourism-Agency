using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class CreateTripDTO
{
    [Required(ErrorMessage = "Trip name cannot be empty")]
    [StringLength(50)]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Slug cannot be empty")]
    [StringLength(100)]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "Availabilty status cannot be empty")]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "Description cannot be empty")]
    public string? Description { get; set; }

    [Required]
    public bool IsPrivate { get; set; }

}
