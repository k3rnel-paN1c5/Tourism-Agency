using System;
using Microsoft.EntityFrameworkCore;
using DataAccess.Contexts;
using DataAccess.Repositories.IRepositories;
using DataAccess.Entities;
using DataAccess.Entities.Enums;
namespace DataAccess.Repositories
{
    public class BookingRepository : Repository<Booking, int>, IBookingRepository
    {
        public BookingRepository(TourismAgencyDbContext context) : base(context) { }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _dbSet
                .Where(b => b.Status == status)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerId)
        {
            return await _dbSet
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Payments)
                .OrderByDescending(b => b.StartDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetEmployeeBookingsAsync(string employeeId)
        {
            return await _dbSet
                .Where(b => b.EmployeeId == employeeId)
                .OrderByDescending(b => b.StartDate)
                .ToListAsync();
        }


        public async Task<Booking?> GetFullBookingDetailsAsync(int bookingId)
        {
            return await _dbSet
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Payments)
                .Include(b => b.CarBooking)
                .Include(b => b.TripBooking)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }
        public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(int daysAhead)
        {
            var dateFrom = DateTime.Now;
            var dateTo = DateTime.Now.AddDays(daysAhead);

            return await _dbSet
                .Where(b => b.StartDate >= dateFrom && b.StartDate <= dateTo)
                .Include(b => b.Customer)
                .Include(b => b.CarBooking)
                .Include(b => b.TripBooking)
                .Where(b => b.Status == BookingStatus.NotStartedYet || 
                           b.Status == BookingStatus.Pending)
                .OrderBy(b => b.StartDate)
                .ToListAsync();
        }


       
    }
}