namespace REST.Publisher.Models.DTOs.Request;

public class NoteRequestDto
{
    public string? Country { get; set; }
    public long Id { get; set; }
    public string? IssueId { get; set; }
    public string? Content { get; set; }
}