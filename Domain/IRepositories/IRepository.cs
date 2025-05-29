using System.Linq.Expressions;

namespace Domain.IRepositories;

/// <summary>
/// Defines a generic repository interface for common data access operations.
/// </summary>
/// <typeparam name="T">The type of the entity (must be a class).</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IRepository<T, TKey> where T : class
{
    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all entities.</returns>
    public Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves all entities that satisfy a specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities that satisfy the predicate.</returns>
    public async Task<IEnumerable<T>> GetAllByPredicateAsync(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
    public Task<T?> GetByIdAsync(TKey id);

    /// <summary>
    /// Retrieves a single entity that satisfies a specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
    public Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(T entity);

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task DeleteByIdAsync(TKey id);

    /// <summary>
    /// Saves all changes made in this context to the underlying data store asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public Task SaveAsync();
}
