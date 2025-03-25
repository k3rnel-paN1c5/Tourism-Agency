using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public enum BookingStatusEnum
    {
        Pending,//waiting to get accepted by an employee
        Denied,
        NotStartedYet,//accepted but not started yet
        InProgress,
        Completed,// or Ended
        Cancelled

    }
    
    public partial class Booking
    {
     public Booking()
        {
            Payments = new HashSet<Payment>();
        }
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("bookingType")]
        public bool BookingType { get; set; }


      
        [Required]
        [Column("startDate", TypeName = "datetime2(7)")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("endDate", TypeName = "datetime2(7)")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("status")]
        [EnumDataType(typeof(BookingStatusEnum))]
        public BookingStatusEnum? Status { get; set; }

        [Required]
        [Column("numOfPassengers")]
        public int NumOfPassengers { get; set; }

        // Navigation Properties
        public CarBooking? CarBooking { get; set; }
        public TripBooking? TripBooking { get; set; }
        public ICollection<Payment> Payments { get; set; }




    }
}
