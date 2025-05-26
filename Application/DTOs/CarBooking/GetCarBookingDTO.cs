using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CarBooking
{
    public class GetCarBookingDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Ending Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Booking Status")]
        public BookingStatus Status { get; set; }
        [Display(Name = "Number of Passengers")]
        public int NumOfPassengers { get; set; }
        [Display(Name = "Customer ID")]
        public string? CustomerId { get; set; }
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }
        [Display(Name = "Car")]
        public string? CarId { get; set; }
        [Display(Name = "Includes a Driver")]
        public bool WithDriver{ get; set; }
        [Display(Name = "Pickup location")]
        public string? PickUpLocation { get; set; }
        [Display(Name = "Dropoff location")]
        public string? DropOffLocation { get; set; }


    }
}
