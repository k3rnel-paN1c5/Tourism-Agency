using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public partial class PaymentTransaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("type")]
        [EnumDataType(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(16,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("transactionDate", TypeName = "datetime2(7)")]
        public DateTime TransactionDate { get; set; }

        [Column("paymentId")]
        [ForeignKey("Payment")]
        public int PaymentId { get; set; }

        [Column("paymentMethodId")]
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }

        // Navigation Properties
        public Payment? Payment { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
