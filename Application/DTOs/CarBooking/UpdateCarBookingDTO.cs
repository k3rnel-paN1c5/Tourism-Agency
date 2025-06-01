using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.DTOs.CarBooking
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for updating an existing car booking.
    /// Used to modify details of a car rental reservation.
    /// </summary>
    public class UpdateCarBookingDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the car booking to be updated. This field is required.
        /// </summary>
        [Required(ErrorMessage = "ID Is required")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the new start date of the car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the new end date of the car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "Ending Date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the new booking status for the car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Booking Status Is required")]
        [Display(Name = "Booking Status")]
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the new number of passengers for the car booking.
        /// Must be at least 1. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Passenger count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger count must be at least 1")]
        [Display(Name = "Number of Passengers")]
        public int NumOfPassengers { get; set; }

        /// <summary>
        /// Gets or sets the new unique identifier of the car being booked. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Car ID is required")]
        [Display(Name = "Car")]
        public int CarId { get; set; }

        /// <summary>
        /// Gets or sets the new pickup location for the car. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Pickup location is required")]
        [Display(Name = "Pickup location")]
        public string? PickUpLocation { get; set; }

        /// <summary>
        /// Gets or sets the new dropoff location for the car. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Dropoff location is required")]
        [Display(Name = "Dropoff location")]
        public string? DropOffLocation { get; set; }

        /// <summary>
        /// Gets or sets the ID of the employee responsible for updating this car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Employee Is required")]
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer associated with this car booking. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Customer Is required")]
        [Display(Name = "Customer ID")]
        public string? CustomerId { get; set; }

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
