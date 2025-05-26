using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TripPlanCar;

public class GetTripPlanCarDTO
{
    [Display(Name = "ID")]
    public int Id { get; set; }
    [Display(Name = "Trip Plan")]
    public int TripPlanId { get; set; }

    [Display(Name = "Car")]
    public int CarId { get; set; }

    [Display(Name = "Price")]
    public decimal Price { get; set; }
}
