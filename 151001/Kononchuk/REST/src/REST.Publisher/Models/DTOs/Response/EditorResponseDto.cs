using System.Text.Json.Serialization;

namespace REST.Publisher.Models.DTOs.Response;

public class EditorResponseDto
{
    public long Id { get; set; }
    public string Login { get; set; }
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastname")]
    public string LastName { get; set; }
}