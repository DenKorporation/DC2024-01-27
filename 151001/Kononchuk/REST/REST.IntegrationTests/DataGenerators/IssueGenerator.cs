using Bogus;
using REST.Models.DTOs.Request;
using REST.Models.Entities;

namespace REST.IntegrationTests.DataGenerators;

public static class IssueGenerator
{
    public static Faker<Issue> CreateIssueFaker()
    {
        return new Faker<Issue>()
            .RuleFor(i => i.Id, 0)
            .RuleFor(i => i.EditorId, value: null)
            .RuleFor(i => i.Title, f => f.Random.Utf16String(2, 64, true))
            .RuleFor(i => i.Content, f => f.Random.Utf16String(4, 2048, true))
            .RuleFor(i => i.Created, f => f.Date.RecentOffset(5).UtcDateTime)
            .RuleFor(i => i.Modified, (f, i) =>
                i.Created.AddTicks(f.Random.Long(0, DateTimeOffset.Now.Ticks - i.Created.Ticks)).UtcDateTime);
    }

    public static Faker<IssueRequestDto> CreateIssueRequestDtoFaker()
    {
        return new Faker<IssueRequestDto>()
            .RuleFor(i => i.Id, 0)
            .RuleFor(i => i.EditorId, value: null)
            .RuleFor(i => i.Title, f => f.Random.Utf16String(2, 64, true))
            .RuleFor(i => i.Content, f => f.Random.Utf16String(4, 2048, true));
    }
}