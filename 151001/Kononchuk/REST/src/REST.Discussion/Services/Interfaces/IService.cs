using REST.Discussion.Exceptions;
using REST.Discussion.Repositories.Interfaces;

namespace REST.Discussion.Services.Interfaces;

public interface IService<TRequest, TResponse>
{
    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <exception cref="ArgumentNullException">entity is null</exception>
    /// <exception cref="ValidationException">Occurs if the entity does not pass validation</exception>
    /// <exception cref="UniqueConstraintException">Occurs if the uniqueness constraint is not met.
    /// See <see cref="IRepository{TKey,TEntity}.AddAsync"/> for the possible source of the exception.</exception>
    public Task<TResponse> CreateAsync(TRequest dto);

    /// <summary>
    /// Returns an entity by given id
    /// </summary>
    /// <exception cref="IRepository{TKey,TEntity}.GetByIdAsync">Occurs if an entity with the specified ID does not exist.
    /// See <see cref="IRepository{TKey,TEntity}"/> for the possible source of the exception.</exception>
    public Task<TResponse> GetByIdAsync(long id);

    public Task<IEnumerable<TResponse>> GetAllAsync();

    /// <summary>
    /// Updates an entity with the given id and the given values
    /// </summary>
    /// <exception cref="ArgumentNullException">entity is null</exception>
    /// <exception cref="ValidationException">Occurs if the entity does not pass validation</exception>
    /// <exception cref="ResourceNotFoundException">Occurs if an entity with the specified ID does not exist.
    /// See <see cref="IRepository{TKey,TEntity}.UpdateAsync"/> for the possible source of the exception.</exception>
    /// <exception cref="UniqueConstraintException">Occurs if the uniqueness constraint is not met.
    /// See <see cref="IRepository{TKey,TEntity}.UpdateAsync"/> for the possible source of the exception.</exception>
    public Task<TResponse> UpdateAsync(long id, TRequest dto);

    /// <summary>
    /// Delete an entity with the given id
    /// </summary>
    /// <exception cref="ResourceNotFoundException">Occurs if an entity with the specified ID does not exist
    /// See <see cref="IRepository{TKey,TEntity}.DeleteAsync"/> for the possible source of the exception.</exception>
    public Task DeleteAsync(long id);
}