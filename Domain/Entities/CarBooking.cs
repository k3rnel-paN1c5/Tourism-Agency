using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
        public partial class CarBooking
    {
        public CarBooking()
        {
            ImageShots = new HashSet<ImageShot>();
        }
        [Key]
        [Column("bookingId")]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required]
        [Column("carId")]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required]
        [Column("pickUpLocation", TypeName = "nvarchar(50)")]
        public string PickUpLocation { get; set; } = string.Empty;


        [Required]
        [Column("dropOffLocation", TypeName = "nvarchar(50)")]
        public string DropOffLocation { get; set; } = string.Empty;

        [Required]
        [Column("withDriver")]
        public bool WithDriver { get; set; }
        
        // Navigation Properties
        public Car? Car { get; set; }
        public Booking? Booking { get; set; }
        public ICollection<ImageShot> ImageShots { get; set; }






    }
}
