using DTO.Booking;
using DTO.Car;
using DTO.ImageShot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CarBooking
{
    public class ReturnCarBookingDTO
    {
        public int CarId { get; set; }
        public string? PickUpLocation { get; set; }
        public string? DropOffLocation { get; set; }
        public ReturnCarDTO? Car { get; set; }
        public ReturnBookingDTO? Booking { get; set; }
        public List<ReturnImageShotDTO> ImageShots { get; set; } = [];
    }
}

