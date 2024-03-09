using REST.Models.Entities;
using REST.Repositories.Interfaces;

namespace REST.Repositories.Implementations.Memory;

public class TagRepository : MemoryRepository<long, Tag>, ITagRepository<long>
{
    private long _globalId;
    
    public override Tag? Add(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        if (Entities.TryAdd(id, entity))
            return entity;

        return null;
    }

    public override Tag? Update(long id, Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        Tag? tag = Entities.FirstOrDefault(pair => pair.Key == id).Value;
        if (tag is not null)
        {
            tag = entity;
            tag.Id = id;
            
            return tag;
        }

        return null;
    }
}