using System;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RegionRepository : Repository<Region, int>, IRegionRepository
{
    public RegionRepository(DbContext context) : base(context){ }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        return await _dbSet.AnyAsync(r => r.Name == name);
    }
}
