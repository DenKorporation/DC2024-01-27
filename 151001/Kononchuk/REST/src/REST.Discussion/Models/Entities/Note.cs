namespace REST.Discussion.Models.Entities;

public class Note
{
    public string Country { get; set; }
    public long? IssueId { get; set; }
    public long Id { get; set; }
    public string Content { get; set; }
}