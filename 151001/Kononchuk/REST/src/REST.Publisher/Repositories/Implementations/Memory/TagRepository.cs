using REST.Publisher.Models.Entities;
using REST.Publisher.Repositories.Interfaces;
using REST.Publisher.Utilities.Exceptions;

namespace REST.Publisher.Repositories.Implementations.Memory;

public class TagRepository : MemoryRepository<long, Tag>, ITagRepository<long>
{
    private long _globalId;
    
    public override Task<Tag> AddAsync(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return Task.FromResult(entity);
    }

    public override async Task<Tag> UpdateAsync(long id, Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (await ExistAsync(id))
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