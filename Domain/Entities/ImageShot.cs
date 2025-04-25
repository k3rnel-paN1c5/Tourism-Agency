using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class ImageShot
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("path", TypeName = "nvarchar(50)")]
        public string Path { get; set; } = string.Empty;

        [Required]
        [Column("type")]
        public bool Type { get; set; }

        // foreign key referencing CarBooking 
        // instead of Booking as shown in diagram
        [Required]
        [Column("carBookingId")]
        [ForeignKey("CarBooking")]
        public int CarBookingId { get; set; }

        // Navigation Properties
        public CarBooking? CarBooking { get; set; }




    }
}
