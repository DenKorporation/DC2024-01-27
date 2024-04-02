using System.Text.Json.Serialization;

namespace REST.Publisher.Models.DTOs.Request;

public class EditorRequestDto
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastname")]
    public string LastName { get; set; }
}