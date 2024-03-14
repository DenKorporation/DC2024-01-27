using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public class IssueRepository : MemoryRepository<long, Issue>, IIssueRepository<long>
{
    private long _globalId;
    
    public override Task<Issue> AddAsync(Issue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return Task.FromResult(entity);
    }

    public override async Task<Issue> UpdateAsync(long id, Issue entity)
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