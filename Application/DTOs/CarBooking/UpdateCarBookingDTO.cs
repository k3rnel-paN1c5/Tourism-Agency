using Domain.Enums;
// using DTO.ImageShot;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CarBooking
{
    public class UpdateCarBookingDTO
    {
        [Required]
        public int BookingId { get; set; }

        public int? CarId { get; set; }

        public string? PickUpLocation { get; set; }
        public string? DropOffLocation { get; set; }

        [Required(ErrorMessage = "Booking status is required")]
        [EnumDataType(typeof(BookingStatus))]
        public BookingStatus? Status { get; set; }

        public bool? WithDriver { get; set; }
        // public List<UpdateImageShotDTO>? ImageShots { get; set; }
    }
}
