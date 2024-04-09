namespace REST.Publisher.Models.DTOs.Response;

public class NoteResponseDto
{
    public long Id { get; set; }
    public long? IssueId { get; set; }
    public string? Content { get; set; }
}