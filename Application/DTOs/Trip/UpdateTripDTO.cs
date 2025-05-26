using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class UpdateTripDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Trip ID")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(50, ErrorMessage = "{0} cannot exceed 50 characters.")]
    [Display(Name = "Trip Name")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(100, ErrorMessage = "{0} cannot exceed 100 characters.")]
    [Display(Name = "Trip Sug")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [Display(Name = "Availablity")]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "{0}  is required.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    [Display(Name = "Private Trip")]
    [Required(ErrorMessage = "Privacy status is required.")]
    public bool IsPrivate { get; set; }
}
