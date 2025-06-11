using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CarBooking
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for retrieving car booking information.
    /// Used to display details of a car rental reservation.
    /// </summary>
    public class GetCarBookingDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the car booking.
        /// </summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the start date of the car booking.
        /// </summary>
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the car booking.
        /// </summary>
        [Display(Name = "Ending Date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the current status of the car booking (e.g., Pending, Confirmed, Cancelled).
        /// </summary>
        [Display(Name = "Booking Status")]
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the number of passengers for the car booking.
        /// </summary>
        [Display(Name = "Number of Passengers")]
        public int NumOfPassengers { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer who made the car booking.
        /// </summary>
        [Display(Name = "Customer ID")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the employee managing this car booking.
        /// </summary>
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the car associated with this booking.
        /// </summary>
        [Display(Name = "Car")]
        public int CarId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the car booking includes a driver.
        /// </summary>
        [Display(Name = "Includes a Driver")]
        public bool WithDriver{ get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the car.
        /// </summary>
        [Display(Name = "Pickup location")]
        public string? PickUpLocation { get; set; }

        /// <summary>
        /// Gets or sets the dropoff location for the car.
        /// </summary>
        [Display(Name = "Dropoff location")]
        public string? DropOffLocation { get; set; }


    }
}
