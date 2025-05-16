using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class CreateTripDTO
{
    [Required(ErrorMessage = "Trip Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string? Name { get; set; }

    [StringLength(100, ErrorMessage = "Slug cannot exceed 100 characters.")]
    public string? Slug { get; set; }

    [Required]
    [DefaultValue(value:true)]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    [Required]
    [DefaultValue(value:false)]
    public bool IsPrivate { get; set; }

}
