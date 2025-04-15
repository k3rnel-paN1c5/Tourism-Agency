using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PaymentTransaction
{
    public class UpdatePaymentTransactionDTO
    {
        [Required(ErrorMessage = "Transaction ID is required")]
        public int Id { get; set; }

        [EnumDataType(typeof(TransactionTypeEnum))]
        public TransactionTypeEnum? TransactionType { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Amount { get; set; }

        public DateTime? TransactionDate { get; set; }

        public int? PaymentId { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}
