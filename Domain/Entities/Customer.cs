using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
        }
        [Key, Column("id", TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }

        [Required]
        [Column("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [Required, Column("lastName")]
        public string LastName { get; set; } = string.Empty;

        [Required, Column("phoneNumber", TypeName = "char(12)")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Column("whatsapp", TypeName = "char(14)")]
        public string Whatsapp { get; set; } = string.Empty;

        [Column("Country")]
        public string Country { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; }
    }
}
