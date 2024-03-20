namespace REST.Models.Entities;

public class Issue
{
    public long Id { get; set; }
    public long? EditorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    
    public Editor? Editor { get; set; }
    public List<Note> Notes { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();

    public List<IssueTag> IssueTags { get; set; } = new();
}