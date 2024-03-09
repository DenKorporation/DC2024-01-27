using REST.Repositories.Interfaces;

namespace REST.Repositories.Implementations.Memory;

public abstract class MemoryRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class where TKey : notnull
{
    protected readonly Dictionary<TKey, TEntity> Entities = new ();

    public abstract TEntity? Add(TEntity entity);

    public bool Exist(TKey id)
    {
        return Entities.ContainsKey(id);
    }

    public TEntity? GetById(TKey id)
    {
        return Entities.FirstOrDefault(pair => pair.Key.Equals(id)).Value;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return Entities.Select(pair => pair.Value);
    }

    public abstract TEntity? Update(TKey id, TEntity entity);

    public bool Delete(TKey id)
    {
        return Exist(id) && Entities.Remove(id);
    }
}