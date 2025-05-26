using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CarBooking
{
    public class CreateCarBookingDTO
    {

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "Ending Date")]
        public DateTime EndDate { get; set; }


        [Required(ErrorMessage = "Passenger count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger count must be at least 1")]
        [Display(Name = "Number of Passengers")]
        public int NumOfPassengers { get; set; }

        [Required(ErrorMessage = "Car ID is required")]
        [Display(Name = "Car")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Pickup location is required")]
        [Display(Name= "Pickup location")]
        public string? PickUpLocation { get; set; }

        [Required(ErrorMessage = "Dropoff location is required")]
        [Display(Name = "Dropoff location")]
        public string? DropOffLocation { get; set; }


        [Required]
        [DefaultValue(value: false)]
        [Display(Name = "Includes a Driver")]
        public bool WithDriver { get; set; }


    }
}
