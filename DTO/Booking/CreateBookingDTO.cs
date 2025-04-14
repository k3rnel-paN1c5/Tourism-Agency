using DTO.CarBooking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Booking
{
    public class CreateBookingDTO
    {
        [Required(ErrorMessage = "Booking type is required")]
        public bool BookingType { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(BookingStatusEnum))]
        public BookingStatusEnum Status { get; set; }

        [Required(ErrorMessage = "Passenger count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger count must be positive")]
        public int NumOfPassengers { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public string? CustomerId { get; set; }
        [Required(ErrorMessage = "Employee ID is required")]
        public string? EmployeeId { get; set; }

        public CreateCarBookingDTO? CarBooking { get; set; }
        // public CreateTripBookingDTO? TripBooking { get; set; }
    }
}

