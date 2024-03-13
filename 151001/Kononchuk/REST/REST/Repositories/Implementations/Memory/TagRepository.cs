using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public class TagRepository : MemoryRepository<long, Tag>, ITagRepository<long>
{
    private long _globalId;
    
    public override Tag Add(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return entity;
    }

    public override Tag Update(long id, Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (Exist(id))
        {
            entity.Id = id;
            Entities[id] = entity;

            return entity;
        }
        else
        {
            throw new ResourceNotFoundException(code: 40402);
        }
    }
}