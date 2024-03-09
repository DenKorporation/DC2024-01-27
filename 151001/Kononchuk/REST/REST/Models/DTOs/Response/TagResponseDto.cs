namespace REST.Models.DTOs.Response;

public class TagResponseDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public long IssueId { get; set; } = -1;
}