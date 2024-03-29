namespace REST.Models.Entities;

public class Tag
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public List<Issue> Issues { get; set; } = new();
    
    public List<IssueTag> IssueTags { get; set; } = new();
}