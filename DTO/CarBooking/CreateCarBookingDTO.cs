using DTO.Booking;
using DTO.ImageShot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CarBooking
{
    public class CreateCarBookingDTO
    {
        [Required]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Car ID is required")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Pickup location is required")]
        public string? PickUpLocation { get; set; }

        [Required(ErrorMessage = "Dropoff location is required")]
        public string? DropOffLocation { get; set; }

        [Required(ErrorMessage = "Driver option is required")]
        public bool WithDriver { get; set; }

        public List<CreateImageShotDTO> ImageShots { get; set; } = new();
    }



}

