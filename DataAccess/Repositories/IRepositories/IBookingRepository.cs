using System;
using DataAccess.Entities;
using DataAccess.Entities.Enums;

namespace DataAccess.Repositories.IRepositories
{

    public interface IBookingRepository : IRepository<Booking, int>
    {
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerId);
        Task<IEnumerable<Booking>> GetEmployeeBookingsAsync(string employeeId);
        Task<Booking?> GetFullBookingDetailsAsync(int bookingId);
        Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(int daysAhead);
    }
}