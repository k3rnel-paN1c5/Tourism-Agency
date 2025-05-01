using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPaymentRepository : IRepository<Payment, int>
{
    Task<Payment?> GetByBookingIdAsync(int bookingId);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
    Task<IEnumerable<Payment>> GetPaymentsWithTransactionsAsync();
}