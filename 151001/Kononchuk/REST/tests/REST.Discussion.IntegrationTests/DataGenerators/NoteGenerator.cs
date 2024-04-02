using Bogus;
using REST.Discussion.Models.DTOs.Request;
using REST.Discussion.Models.Entities;

namespace REST.Discussion.IntegrationTests.DataGenerators;

public static class NoteGenerator
{
    public static Faker<Note> CreateNoteFaker()
    {
        return new Faker<Note>()
            .RuleFor(n => n.Id, 0)
            .RuleFor(n => n.IssueId,  0)
            .RuleFor(n => n.Content, f => f.Random.Utf16String(2, 2048, true));
    }

    public static Faker<NoteRequestDto> CreateNoteRequestDtoFaker()
    {
        return new Faker<NoteRequestDto>()
            .RuleFor(n => n.Id, 0)
            .RuleFor(n => n.IssueId,  0)
            .RuleFor(n => n.Content, f => f.Random.Utf16String(2, 2048, true));
    }
}