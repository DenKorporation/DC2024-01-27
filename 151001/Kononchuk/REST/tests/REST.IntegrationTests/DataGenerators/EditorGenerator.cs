using Bogus;
using REST.Models.DTOs.Request;
using REST.Models.Entities;

namespace REST.IntegrationTests.DataGenerators;

public static class EditorGenerator
{
    public static Faker<Editor> CreateEditorFaker()
    {
        return new Faker<Editor>()
            .RuleFor(e => e.Id, 0)
            .RuleFor(e => e.Login, f => f.Internet.Email())
            .RuleFor(e => e.Password, f => f.Internet.Password())
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName());
    }

    public static Faker<EditorRequestDto> CreateEditorRequestDtoFaker()
    {
        return new Faker<EditorRequestDto>()
            .RuleFor(e => e.Id, value: 0)
            .RuleFor(e => e.Login, f => f.Internet.Email())
            .RuleFor(e => e.Password, f => f.Internet.Password())
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName());
    }
}