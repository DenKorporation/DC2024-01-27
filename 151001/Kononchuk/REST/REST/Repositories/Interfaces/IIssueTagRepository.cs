using REST.Models.Entities;

namespace REST.Repositories.Interfaces;

public interface IIssueTagRepository<TKey> : IRepository<TKey, IssueTag>
{
    IEnumerable<IssueTag> GetByIssueId(TKey id);
    IEnumerable<IssueTag> GetByTagId(TKey id);
}