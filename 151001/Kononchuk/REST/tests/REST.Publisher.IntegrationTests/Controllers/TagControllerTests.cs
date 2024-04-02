using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using REST.Publisher.Data;
using REST.Publisher.IntegrationTests.DataGenerators;
using REST.Publisher.IntegrationTests.Fixtures;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Models.Entities;

namespace REST.Publisher.IntegrationTests.Controllers;

[Collection("Controller Collection")]
public class TagControllerTests(RestWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly AppDbContext _dbContext =
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    private readonly Func<Task> _resetDatabase = factory.ResetDatabase;
    private readonly HttpClient _client = factory.HttpClient;
    private const string BasePath = "tags";

    private readonly Faker<TagRequestDto> _tagRequestGenerator = TagGenerator.CreateTagRequestDtoFaker();
    private readonly Faker<Tag> _tagGenerator = TagGenerator.CreateTagFaker();
    
    [Fact]
    public async Task Create_ValidData_ReturnsCreatedWithCreatedTag()
    {
        TagRequestDto requestDto = _tagRequestGenerator.Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);

        var responseDto = await response.Content.ReadFromJsonAsync<TagResponseDto>();
        responseDto!.Id.Should().BePositive();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(requestDto,
            options => options.ExcludingMissingMembers()
                .Excluding(t => t.Id));
    }
    
    [Fact]
    public async Task Create_NonUniqueValue_ReturnsForbiddenStatus()
    {
        Tag createTag = _tagGenerator.Generate();
        _dbContext.Add(createTag);
        await _dbContext.SaveChangesAsync();
        TagRequestDto requestDto = _tagRequestGenerator.Clone()
            .RuleFor(t => t.Name, createTag.Name).Generate();

        
        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40301);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        TagRequestDto requestDto = _tagRequestGenerator.Clone().RuleFor(t => t.Name, "").Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40001);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ValidState_ReturnsOkWithListOfTags()
    {
        var response = await _client.GetAsync(BasePath);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        var responseDto = await response.Content.ReadFromJsonAsync<List<TagResponseDto>>();
        responseDto.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_TagNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BasePath}/-1");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);


        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40401);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_TagExist_ReturnsExistingResult()
    {
        Tag tag = _tagGenerator.Generate();
        _dbContext.Add(tag);
        await _dbContext.SaveChangesAsync();


        var response = await _client.GetAsync($"{BasePath}/{tag.Id}");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<TagResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(tag,
            options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_TagNotExist_ReturnsNotFound()
    {
        TagRequestDto requestDto = _tagRequestGenerator.Clone()
            .RuleFor(t => t.Id, -1).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, requestDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40402);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_ValidData_ReturnsOkWithUpdatedTag()
    {
        Tag createTag = _tagGenerator.Generate();
        _dbContext.Add(createTag);
        await _dbContext.SaveChangesAsync();

        TagRequestDto updateDto = _tagRequestGenerator.Clone()
            .RuleFor(t => t.Id, createTag.Id).Generate();
        

        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<TagResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(updateDto,
            options => options.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task Update_NonUniqueValue_ReturnsForbiddenStatus()
    {
        Tag createTag = _tagGenerator.Generate();
        _dbContext.Add(createTag);
        await _dbContext.SaveChangesAsync();
        TagRequestDto updateDto = _tagRequestGenerator.Clone()
            .RuleFor(t => t.Name, createTag.Name).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40302);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_InvalidData_ReturnsBadRequest()
    {
        Tag createTag = _tagGenerator.Generate();
        _dbContext.Add(createTag);
        await _dbContext.SaveChangesAsync();

        TagRequestDto updateDto = _tagRequestGenerator.Clone()
            .RuleFor(t => t.Id, createTag.Id)
            .RuleFor(t => t.Name, "").Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40002);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_TagNotExist_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{BasePath}/-1");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40403);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_TagExist_ReturnsNoContent()
    {
        Tag createTag = _tagGenerator.Generate();
        _dbContext.Add(createTag);
        await _dbContext.SaveChangesAsync();

        
        var response = await _client.DeleteAsync($"{BasePath}/{createTag.Id}");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}