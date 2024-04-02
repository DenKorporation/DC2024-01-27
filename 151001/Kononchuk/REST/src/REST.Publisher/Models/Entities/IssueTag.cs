namespace REST.Publisher.Models.Entities;

public class IssueTag
{
    public long Id { get; set; }
    
    public long IssueId { get; set; }
    public Issue? Issue { get; set; }
    
    public long TagId { get; set; }
    public Tag? Tag { get; set; }
}