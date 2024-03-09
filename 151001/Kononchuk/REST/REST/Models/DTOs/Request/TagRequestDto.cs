namespace REST.Models.DTOs.Request;

public class TagRequestDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public long IssueId { get; set; } = -1;
}