using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CarBooking
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for creating a new car booking.
    /// Used to capture detailed information required for a car rental reservation.
    /// </summary>
    public class CreateCarBookingDTO
    {
        /// <summary>
        /// Gets or sets the start date of the car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "Ending Date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of passengers for the car booking.
        /// Must be at least 1. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Passenger count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger count must be at least 1")]
        [Display(Name = "Number of Passengers")]
        public int NumOfPassengers { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the car being booked. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Car ID is required")]
        [Display(Name = "Car")]
        public int CarId { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the car. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Pickup location is required")]
        [Display(Name= "Pickup location")]
        public string? PickUpLocation { get; set; }

        /// <summary>
        /// Gets or sets the dropoff location for the car. This field is required.
        /// </summary
        [Required(ErrorMessage = "Dropoff location is required")]
        [Display(Name = "Dropoff location")]
        public string? DropOffLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the car booking includes a driver.
        /// Defaults to false. This field is required.
        /// </summary>
        [Required]
        [DefaultValue(value: false)]
        [Display(Name = "Includes a Driver")]
        public bool WithDriver { get; set; }


    }
}
