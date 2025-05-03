using System;
using System.Data.Common;
using Domain.Entities;

namespace Domain.IRepositories;

public interface IRegionRepository : IRepository<Region, int>
{
    Task<bool> ExistsByNameAsync(string name);
}
