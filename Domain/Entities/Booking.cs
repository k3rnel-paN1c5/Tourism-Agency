using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{

    public partial class Booking
    {

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
        [EnumDataType(typeof(BookingStatus))]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Required]
        [Column("numOfPassengers")]
        public int NumOfPassengers { get; set; }

        [Required]
        [Column("customerId")]
        [ForeignKey("Customer")]
        public string? CustomerId { get; set; }

        [Column("employeeId")]
        [ForeignKey("Employee")]
        public string? EmployeeId { get; set; } 

        [Column("paymentId")]
        [ForeignKey("Payment")]
        public int? PaytmentId { get; set; } 

        // Navigation Properties
        public Employee? Employee { get; set; }
        public Customer? Customer { get; set; }
        public Payment? Payment { get; set; }
        public CarBooking? CarBooking { get; set; }
        public TripBooking? TripBooking { get; set; }




    }
}
