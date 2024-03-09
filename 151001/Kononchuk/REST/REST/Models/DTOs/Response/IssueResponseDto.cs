namespace REST.Models.DTOs.Response;

public class IssueResponseDto
{
    public long Id { get; set; }
    public long? EditorId { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
}