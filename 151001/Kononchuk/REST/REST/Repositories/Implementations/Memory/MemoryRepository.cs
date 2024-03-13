using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public abstract class MemoryRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class where TKey : notnull
{
    protected readonly Dictionary<TKey, TEntity> Entities = new ();

    public abstract TEntity Add(TEntity entity);

    public bool Exist(TKey id)
    {
        return Entities.ContainsKey(id);
    }

    public TEntity GetById(TKey id)
    {
        if (Exist(id))
        {
            return Entities[id];
        }
        else
        {
            throw new ResourceNotFoundException(code: 40401);
        }
    }

    public IEnumerable<TEntity> GetAll()
    {
        return Entities.Select(pair => pair.Value);
    }

    public abstract TEntity Update(TKey id, TEntity entity);

    public void Delete(TKey id)
    {
        if (Exist(id))
        {
            Entities.Remove(id);
        }
        else
        {
            throw new ResourceNotFoundException(code: 40403);
        }
    }
}