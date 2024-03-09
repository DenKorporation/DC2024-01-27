namespace REST.Repositories.Interfaces;

public interface IRepository<TKey, TEntity> where TEntity : class
{
    // Create
    TEntity? Add(TEntity entity);

    // Read
    bool Exist(TKey id);
    TEntity? GetById(TKey id);
    IEnumerable<TEntity> GetAll();
    
    // Update
    TEntity? Update(TKey id, TEntity entity);

    // Delete
    bool Delete(TKey id);
}