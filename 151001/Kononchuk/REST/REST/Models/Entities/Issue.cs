namespace REST.Models.Entities;

public class Issue
{
    public long Id { get; set; }
    public long EditorId { get; set; } = -1;
    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
}