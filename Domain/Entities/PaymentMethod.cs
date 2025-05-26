using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            PaymentTransactions = new HashSet<PaymentTransaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("method", TypeName = "nvarchar(50)")]
        public string? Method { get; set; }

        [Column("icon", TypeName = "nvarchar(50)")]
        public string Icon { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
