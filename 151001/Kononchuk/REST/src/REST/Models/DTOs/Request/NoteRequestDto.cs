namespace REST.Models.DTOs.Request;

public class NoteRequestDto
{
    public long Id { get; set; }
    public long? IssueId { get; set; }
    public string? Content { get; set; }
}