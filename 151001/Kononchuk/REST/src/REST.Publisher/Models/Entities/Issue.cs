namespace REST.Publisher.Models.Entities;

public class Issue
{
    public long Id { get; set; }
    public long? EditorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    
    public Editor? Editor { get; set; }
    public List<Tag> Tags { get; set; } = new();

    public List<IssueTag> IssueTags { get; set; } = new();
}