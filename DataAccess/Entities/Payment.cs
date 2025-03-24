using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public enum StatusEnum { 
        Pending,
        Complete,
        Refund
    }
    public partial class Payment
    {
        public Payment()
        {
            Payments = new HashSet<PaymentTransaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("status")][EnumDataType(typeof(StatusEnum))]
        public StatusEnum Status { get; set; }

        [Required]
        [Column("amountDue", TypeName = "decimal(16,2)")]
        public decimal AmountDue { get; set; }

        [Required]
        [Column("amountPaid", TypeName = "decimal(16,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Column("paymentDate", TypeName = "datetime2(7)")]
        public DateTime PaymentDate { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        public virtual ICollection<PaymentTransaction> Payments { get; set; }
    }
}
