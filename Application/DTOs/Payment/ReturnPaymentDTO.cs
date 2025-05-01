using DataAccess.Entities.Enums;
using DTO.PaymentTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Payment
{
    public class ReturnPaymentDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }

        // Nested DTOs
        public List<ReturnPaymentTransactionDTO> Transactions { get; set; } = [];
        
    }
}
