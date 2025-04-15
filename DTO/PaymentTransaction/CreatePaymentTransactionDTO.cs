using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PaymentTransaction
{
    public class CreatePaymentTransactionDTO
    {
        [Required(ErrorMessage = "Transaction type is required")]
        [EnumDataType(typeof(TransactionTypeEnum), ErrorMessage = "Invalid transaction type")]
        public TransactionTypeEnum TransactionType { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction date is required")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Payment ID is required")]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Payment method ID is required")]
        public int PaymentMethodId { get; set; }
    }
}
