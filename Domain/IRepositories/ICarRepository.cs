using Domain.Entities;
namespace Domain.IRepositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        Task<IEnumerable<int>> GetAvailableCarsAsync(DateTime start, DateTime end);
    }
}
