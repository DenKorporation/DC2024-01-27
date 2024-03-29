using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public abstract class MemoryRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class where TKey : notnull
{
    protected readonly Dictionary<TKey, TEntity> Entities = new ();

    public abstract Task<TEntity> AddAsync(TEntity entity);

    public Task<bool> ExistAsync(TKey id)
    {
        return Task.FromResult(Entities.ContainsKey(id));
    }

    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        if (await ExistAsync(id))
        {
            return Entities[id];
        }
        else
        {
            throw new ResourceNotFoundException(code: 40401);
        }
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Task.FromResult(Entities.Select(pair => pair.Value));
    }

    public abstract Task<TEntity> UpdateAsync(TKey id, TEntity entity);

    public async Task DeleteAsync(TKey id)
    {
        if (await ExistAsync(id))
        {
            Entities.Remove(id);
        }
        else
        {
            throw new ResourceNotFoundException(code: 40403);
        }
    }
}