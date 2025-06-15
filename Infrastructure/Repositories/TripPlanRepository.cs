using System;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories;

public class TripPlanRepository : Repository<TripPlan,int>, ITripPlanRepository
{
    public TripPlanRepository(DbContext context): base(context)
    {
    }

    
    public async Task<TripPlan> GetByIdAsync(int id)
    {
        return await _dbSet.Where(t => t.Id == id).Include(p => p.Region).Include(p => p.Trip).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<TripPlan>> GetUpcomingAsync()
    {
        return await _dbSet.Where(t => t.StartDate > DateTime.UtcNow).Include(p => p.Trip).Include(p => p.Region).ToListAsync();
    }
}
