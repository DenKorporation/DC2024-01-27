using REST.Discussion.Models.Entities;

namespace REST.Discussion.Repositories.Interfaces;

public interface INoteRepository<TKey> : IRepository<TKey, Note>
{
    
}