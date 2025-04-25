using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{

    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<PaymentTransaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("bookingId")]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required]
        [Column("status")]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus Status { get; set; }

        [Required]
        [Column("amountDue", TypeName = "decimal(16,2)")]
        public decimal AmountDue { get; set; }

        [Required]
        [Column("amountPaid", TypeName = "decimal(16,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Column("paymentDate", TypeName = "datetime2(7)")]
        public DateTime PaymentDate { get; set; }

        [Column("notes", TypeName = "nvarchar(200)")]
        public string Notes { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<PaymentTransaction> Transactions { get; set; }
        public Booking? Booking { get; set; }
    }
}
