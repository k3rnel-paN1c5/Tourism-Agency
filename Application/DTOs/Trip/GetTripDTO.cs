using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Trip;

public class GetTripDTO
{
    [Display(Name = "Trip ID")]
    public int Id { get; set; }
    [Display(Name = "Trip Name")]
    public string? Name {get; set; }
    [Display(Name = "Trip Slug")]
    public string? Slug { get; set; }
    [Display(Name = "Description")]
    public string? Description { get; set; }
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; }
    [Display(Name = "Private Trip")]
    public bool IsPrivate { get; set; }

}
