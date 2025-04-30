using DataAccess.Entities.Enums;
using DTO.CarBooking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Booking
{
   public class UpdateBookingDTO
    {
        [Required(ErrorMessage = "Booking ID is required")]
        public int Id { get; set; }

        public bool BookingType { get; set; }

       public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [EnumDataType(typeof(BookingStatus))]
        public BookingStatus? Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Passengers must be positive")]
        public int NumOfPassengers { get; set; }

        public string CustomerId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public UpdateCarBookingDTO? CarBooking { get; set; }
        // public UpdateTripBookingDTO? TripBooking { get; set; }
    }
}


