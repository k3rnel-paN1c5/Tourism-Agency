using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public enum CarBookingStatusEnum
    {
        Pending,//waiting to get accepted by an employee
        Denied,
        NotStartedYet,//accepted but not started yet
        InProgress,
        Completed,// or Ended
        Cancelled

    }
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
        public string? PickUpLocation { get; set; }


        [Required]
        [Column("dropOffLocation", TypeName = "nvarchar(50)")]
        public string? DropOffLocation{ get; set; }


        [Required]
        [Column("status")]
        [EnumDataType(typeof(CarBookingStatusEnum))]
        public CarBookingStatusEnum? Status { get; set; }

        [Required]
        [Column("withDriver")]
        public bool WithDriver { get; set; }

        // Navigation Properties
        public Car? Car { get; set; }
        public Booking? Booking { get; set; }
        public ICollection<ImageShot> ImageShots { get; set; }






    }
}
