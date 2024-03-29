namespace REST.Models.Entities;

public class Note
{
    public long Id { get; set; }
    public long IssueId { get; set; } = -1;
    public string Content { get; set; }
    
    public Issue? Issue { get; set; }
}