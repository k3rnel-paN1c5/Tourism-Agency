using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
namespace Infrastructure.Repositories
{
    public class CarRepository : Repository<Car, int>, ICarRepository
    {
        // private readonly TourismAgencyDbContext _appContext;
        public CarRepository(TourismAgencyDbContext context) : base(context) { }
        public async Task<IEnumerable<int>> GetAvailableCarsAsync(DateTime start, DateTime end)
        {
            var unavailableCarIds = await _dbSet
                .Where(c => c.CarBookings.Any(cb =>
                    (cb!.Booking!.StartDate < end && cb!.Booking!.EndDate > start) &&
                    (cb.Booking.Status == BookingStatus.NotStartedYet ||
                     cb.Booking.Status == BookingStatus.InProgress)))
                .Select(c => c.Id)
                .ToListAsync();

            // Return available cars (those not in the unavailable list)
            return await _dbSet
                .Where(c => !unavailableCarIds.Contains(c.Id))
                .Include(c => c.Category)
                .Select(c => c.Id)
                .ToListAsync();
        }
    }
}