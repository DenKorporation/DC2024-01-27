namespace REST.Models.Entities;

public class Editor
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<Issue> Issues { get; set; } = new();
}