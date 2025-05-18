using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripPlanCar;

public class UpdateTripPlanCarDTO
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public int TripPlanId { get; set; }

    [Required]
    public int CarId { get; set; }

    [Required]
    public decimal Price { get; set; }
}
