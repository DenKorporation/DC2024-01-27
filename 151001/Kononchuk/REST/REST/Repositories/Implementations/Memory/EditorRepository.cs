using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public class EditorRepository : MemoryRepository<long, Editor>, IEditorRepository<long>
{
    private long _globalId;
    
    public override Editor Add(Editor entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return entity;
    }

    public override Editor Update(long id, Editor entity)
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