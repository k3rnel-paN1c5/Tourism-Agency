using DataAccess.Entities.Enums;
using DTO.CarBooking;
using DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Booking
{
    public class ReturnBookingDTO
    {
        public int Id { get; set; }
        public bool BookingType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public int NumOfPassengers { get; set; }
        // public ReturnCustomerDTO Customer { get; set; }
        // public ReturnEmployeeDTO Employee { get; set; }
       
        public List<ReturnPaymentDTO> Payments { get; set; } = [];
    }
}

