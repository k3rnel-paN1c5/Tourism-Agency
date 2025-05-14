using Domain.Entities;
using Domain.Enums;

namespace Domain.IRepositories
{
    public interface IPaymentRepository : IRepository<Payment, int>
    {
        Task<Payment?> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetPaymentsWithTransactionsAsync();
    }
}