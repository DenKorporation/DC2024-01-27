namespace REST.Models.Entities;

public class IssueTag
{
    public long Id { get; set; }
    public long IssueId { get; set; }
    public long TagId { get; set; }
}