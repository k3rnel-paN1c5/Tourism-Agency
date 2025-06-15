using System;
using Domain.Entities;

namespace Domain.IRepositories;

/// <summary>
/// Defines a repository interface for TripPlan Access Operations
/// </summary>
public interface ITripPlanRepository : IRepository<TripPlan, int>
{
    /// <summary>
    /// Retrieves all upcoming TripPlans with eager loading asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all Upcoming trip plans.</returns>
    public Task<IEnumerable<TripPlan>> GetUpcomingAsync();
}
