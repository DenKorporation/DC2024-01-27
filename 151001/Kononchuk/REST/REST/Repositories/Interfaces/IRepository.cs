using REST.Utilities.Exceptions;

namespace REST.Repositories.Interfaces;

public interface IRepository<TKey, TEntity> where TEntity : class
{
    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <exception cref="UniqueConstraintException">Occurs if the uniqueness constraint is not met</exception>
    /// <exception cref="ArgumentNullException">entity is null</exception>>
    TEntity Add(TEntity entity);

    // Read
    bool Exist(TKey id);
    
    /// <summary>
    /// Returns an entity by given id
    /// </summary>
    /// <exception cref="ResourceNotFoundException">Occurs if an entity with the specified ID does not exist</exception>
    TEntity GetById(TKey id);
    
    IEnumerable<TEntity> GetAll();
    
    /// <summary>
    /// Updates an entity with the given id and the given values
    /// </summary>
    /// <exception cref="ResourceNotFoundException">Occurs if an entity with the specified ID does not exist</exception>
    /// <exception cref="UniqueConstraintException">Occurs if the uniqueness constraint is not met</exception>
    /// <exception cref="ArgumentNullException">entity is null</exception>>
    TEntity Update(TKey id, TEntity entity);

    /// <summary>
    /// Delete an entity with the given id
    /// </summary>
    /// <exception cref="ResourceNotFoundException">Occurs if an entity with the specified ID does not exist</exception>
    void Delete(TKey id);
}