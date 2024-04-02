namespace REST.Publisher.Models.DTOs.Response;

public class ErrorResponseDto
{
    public int ErrorCode { get; init; }
    public string? ErrorMessage { get; init; }
    
}