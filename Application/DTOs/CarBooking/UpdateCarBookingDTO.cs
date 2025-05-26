using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.DTOs.CarBooking
{
   public class UpdateCarBookingDTO
    {
        [Required(ErrorMessage = "ID Is required")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "Ending Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Booking Status Is required")]
        [Display(Name = "Booking Status")]
        public BookingStatus Status { get; set; }

        [Required(ErrorMessage = "Passenger count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger count must be at least 1")]
        [Display(Name = "Number of Passengers")]
        public int NumOfPassengers { get; set; }

        [Required(ErrorMessage = "Car ID is required")]
        [Display(Name = "Car")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Pickup location is required")]
        [Display(Name = "Pickup location")]
        public string? PickUpLocation { get; set; }

        [Required(ErrorMessage = "Dropoff location is required")]
        [Display(Name = "Dropoff location")]
        public string? DropOffLocation { get; set; }

        [Required(ErrorMessage = "Employee Is required")]
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }

        [Required(ErrorMessage = "Customer Is required")]
        [Display(Name = "Customer ID")]
        public string? CustomerId { get; set; }

        [Required]
        [DefaultValue(value: false)]
        [Display(Name = "Includes a Driver")]
        public bool WithDriver { get; set; }
    }
}
