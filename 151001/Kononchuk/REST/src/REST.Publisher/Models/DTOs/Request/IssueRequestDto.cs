namespace REST.Publisher.Models.DTOs.Request;

public class IssueRequestDto
{
    public long Id { get; set; }
    public long? EditorId { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
}