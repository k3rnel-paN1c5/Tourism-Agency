using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

/// <summary>
/// Implements a generic repository for common data access operations, interacting with a DbContext.
/// </summary>
/// <typeparam name="T">The type of the entity (must be a class).</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public class Repository<T, TKey> : IRepository<T, TKey> where T : class
{
    /// <summary>
    /// The database context used by the repository.
    /// </summary>
    protected readonly DbContext _context;
    /// <summary>
    /// The DbSet for the entity type managed by this repository.
    /// </summary>
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{T, TKey}"/> class.
    /// </summary>
    /// <param name="context">The DbContext instance to use.</param>
    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Retrieves all entities that satisfy a specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities that satisfy the predicate.</returns>
    public async Task<IEnumerable<T>> GetAllByPredicateAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
    public async Task<T?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Retrieves a single entity that satisfies a specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteByIdAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            Delete(entity);
    }
    /// <summary>
    /// Saves all changes made in this context to the underlying data store asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

