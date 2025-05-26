using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class CreateTripDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot exceed 50 characters.")]
    [Display(Name = "Trip Name")]
    public string? Name { get; set; }

    [StringLength(100, ErrorMessage = "{0} cannot exceed 100 characters.")]
    [Display(Name = "Trip Slug")]
    public string? Slug { get; set; }

    [Required]
    [DefaultValue(value:true)]
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required]
    [DefaultValue(value:false)]
    [Display(Name = "Private Trip")]
    public bool IsPrivate { get; set; }

}
