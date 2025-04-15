using DTO.CarBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ImageShot
{
    public class ReturnImageShotDTO
    {
        public int Id { get; set; }
        public string Image { get; set; } // Full URL to access the image
        public string Type { get; set; } // "Pickup" or "Return"
        public ReturnCarBookingDTO CarBooking { get; set; }
    }
}
