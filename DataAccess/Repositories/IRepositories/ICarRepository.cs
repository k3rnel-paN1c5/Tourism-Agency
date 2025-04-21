using System;
using DataAccess.Entities;
namespace DataAccess.Repositories.IRepositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        Task<IEnumerable<int>> GetAvailableCarsAsync(DateTime start, DateTime end);
    }
}
