using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class TripBooking
    {
        [Key]
        [Column("bookingId")]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required]
        [Column("tripPlanId")]
        [ForeignKey("TripPlan")]
        public int TripPlanId { get; set; }

        [Required]
        [Column("withGuide")]
        public bool WithGuide { get; set; }

        //Navigation Properties
        public TripPlan? TripPlan { get; set; }
        public Booking? Booking { get; set; }

    }
}
