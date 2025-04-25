using Domain.Entities;
using Domain.Enums;

namespace Domain.IRepositories
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