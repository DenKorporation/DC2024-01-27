using REST.Models.Entities;

namespace REST.Repositories.Interfaces;

public interface INoteRepository<TKey> : IRepository<TKey, Note>
{
    
}