using DataAccess.Entities.Enums;
using DTO.Payment;
using DTO.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PaymentTransaction
{
    public class ReturnPaymentTransactionDTO
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        // Nested DTOs (avoid circular references)
        public ReturnPaymentDTO? Payment { get; set; }
        public ReturnPaymentMethodDTO? PaymentMethod { get; set; }
    }
}
