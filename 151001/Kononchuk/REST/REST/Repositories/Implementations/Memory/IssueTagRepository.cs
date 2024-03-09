using REST.Models.Entities;
using REST.Repositories.Interfaces;

namespace REST.Repositories.Implementations.Memory;

public class IssueTagRepository: MemoryRepository<long, IssueTag>, IIssueTagRepository<long>
{
    private long _globalId;
    
    public override IssueTag? Add(IssueTag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        if (Entities.TryAdd(id, entity))
            return entity;

        return null;
    }

    public override IssueTag? Update(long id, IssueTag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        IssueTag? issueTag = Entities.FirstOrDefault(pair => pair.Key == id).Value;
        if (issueTag is not null)
        {
            issueTag = entity;
            issueTag.Id = id;
            
            return issueTag;
        }

        return null;
    }

    public IEnumerable<IssueTag> GetByIssueId(long id)
    {
        return Entities.Where(pair => pair.Value.IssueId == id).Select(pair => pair.Value);
    }

    public IEnumerable<IssueTag> GetByTagId(long id)
    {
        return Entities.Where(pair => pair.Value.TagId == id).Select(pair => pair.Value);
    }
}