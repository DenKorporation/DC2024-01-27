using Bogus;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.Entities;

namespace REST.Publisher.IntegrationTests.DataGenerators;

public static class TagGenerator
{
    public static Faker<Tag> CreateTagFaker()
    {
        return new Faker<Tag>()
            .RuleFor(t => t.Id, 0)
            .RuleFor(t => t.Name, f => f.Random.Utf16String(2, 32, true));
    }

    public static Faker<TagRequestDto> CreateTagRequestDtoFaker()
    {
        return new Faker<TagRequestDto>()
            .RuleFor(t => t.Id, value: 0)
            .RuleFor(t => t.Name, f => f.Random.Utf16String(2,32, true));
    }
}