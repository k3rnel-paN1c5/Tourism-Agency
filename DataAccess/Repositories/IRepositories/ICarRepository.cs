using System;
using DataAccess.Entities;
namespace DataAccess.Repositories.IRepositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime start, DateTime end);
    }
}
