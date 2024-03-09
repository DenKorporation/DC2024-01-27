using REST.Models.Entities;
using REST.Repositories.Interfaces;

namespace REST.Repositories.Implementations.Memory;

public class IssueRepository : MemoryRepository<long, Issue>, IIssueRepository<long>
{
    private long _globalId;
    
    public override Issue? Add(Issue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        if (Entities.TryAdd(id, entity))
            return entity;

        return null;
    }

    public override Issue? Update(long id, Issue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        Issue? issue = Entities.FirstOrDefault(pair => pair.Key == id).Value;
        if (issue is not null)
        {
            issue = entity;
            issue.Id = id;
            
            return issue;
        }

        return null;
    }
}