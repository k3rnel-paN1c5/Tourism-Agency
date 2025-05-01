using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPaymentService
{
    Task<Payment> CreatePaymentAsync(int bookingId, decimal amountDue);
    Task<Payment> GetPaymentByIdAsync(int paymentId);
    Task<Payment> GetPaymentByBookingIdAsync(int bookingId);
    Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
    Task<Payment> ProcessRefundAsync(int paymentId, decimal refundAmount, string reason);
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
}