using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Payment
{
    public class UpdatePaymentDTO
    {
        [Required]
        public int Id { get; set; }

        public int? BookingId { get; set; }

        [EnumDataType(typeof(PaymentStatusEnum))]
        public PaymentStatusEnum? Status { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? AmountDue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? AmountPaid { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }

        public List<PaymentTransactionToUpdateDTO>? Transactions { get; set; }
    }
}
