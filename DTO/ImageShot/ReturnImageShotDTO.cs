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
        public string Image { get; set; } = string.Empty;// Full URL to access the image
        public string Type { get; set; } = string.Empty;// "Pickup" or "Return"
        public ReturnCarBookingDTO? CarBooking { get; set; }
    }
}
