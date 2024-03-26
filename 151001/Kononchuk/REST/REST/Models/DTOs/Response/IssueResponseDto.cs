namespace REST.Models.DTOs.Response;

public class IssueResponseDto
{
    public long Id { get; set; }
    public long? EditorId { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset? Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}