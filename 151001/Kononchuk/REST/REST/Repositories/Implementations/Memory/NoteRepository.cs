using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public class NoteRepository : MemoryRepository<long, Note>, INoteRepository<long>
{
    private long _globalId;
    
    public override Note Add(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return entity;
    }

    public override Note Update(long id, Note entity)
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