using DataAccess.Entities.Enums;
using DTO.PaymentTransaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Payment
{
   public class CreatePaymentDTO
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus Status { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal AmountDue { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }

        public List<CreatePaymentTransactionDTO> Transactions { get; set; } = [];
    }

    
}
