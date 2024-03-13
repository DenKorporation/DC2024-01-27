using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public class IssueRepository : MemoryRepository<long, Issue>, IIssueRepository<long>
{
    private long _globalId;
    
    public override Issue Add(Issue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return entity;
    }

    public override Issue Update(long id, Issue entity)
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