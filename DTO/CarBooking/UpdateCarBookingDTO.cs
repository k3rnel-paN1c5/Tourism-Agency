using DataAccess.Entities.Enums;
using DTO.ImageShot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CarBooking
{
    public class UpdateCarBookingDTO
    {
        [Required]
        public int BookingId { get; set; }

        public int? CarId { get; set; }

        public string? PickUpLocation { get; set; }
        public string? DropOffLocation { get; set; }

        [Required(ErrorMessage = "Driver option is required")]
        [EnumDataType(typeof(CarBookingStatus))]
        public CarBookingStatus? Status { get; set; }

        public bool? WithDriver { get; set; }
        public List<UpdateImageShotDTO>? ImageShots { get; set; }
    }
}
