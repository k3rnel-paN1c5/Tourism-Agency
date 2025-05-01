using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PaymentRepository : Repository<Payment, int>, IPaymentRepository
{
    private readonly TourismAgencyDbContext _context;

    public PaymentRepository(TourismAgencyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByBookingIdAsync(int bookingId)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
    {
        return await _dbSet
            .Where(p => p.Status == status)
            .Include(p => p.Booking)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsWithTransactionsAsync()
    {
        return await _dbSet
            .Include(p => p.Transactions)
            .ToListAsync();
    }
}