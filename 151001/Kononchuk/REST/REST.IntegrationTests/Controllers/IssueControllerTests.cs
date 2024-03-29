using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using REST.Data;
using REST.IntegrationTests.DataGenerators;
using REST.IntegrationTests.Fixtures;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;

namespace REST.IntegrationTests.Controllers;

[Collection("Controller Collection")]
public class IssueControllerTests(RestWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly AppDbContext _dbContext =
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    private readonly Func<Task> _resetDatabase = factory.ResetDatabase;
    private readonly HttpClient _client = factory.HttpClient;
    private const string BasePath = "issues";

    private readonly Faker<IssueRequestDto> _issueRequestGenerator = IssueGenerator.CreateIssueRequestDtoFaker();
    private readonly Faker<Issue> _issueGenerator = IssueGenerator.CreateIssueFaker();

    [Fact]
    public async Task Create_ValidData_ReturnsCreatedWithCreatedIssue()
    {
        IssueRequestDto requestDto = _issueRequestGenerator.Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);

        var responseDto = await response.Content.ReadFromJsonAsync<IssueResponseDto>();
        responseDto!.Id.Should().BePositive();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(requestDto,
            options => options.ExcludingMissingMembers()
                .Excluding(i => i.Id));
    }
    
    [Fact]
    public async Task Create_NonUniqueValue_ReturnsForbiddenStatus()
    {
        Issue createIssue = _issueGenerator.Generate();
        _dbContext.Add(createIssue);
        await _dbContext.SaveChangesAsync();
        IssueRequestDto requestDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.Title, createIssue.Title).Generate();

        
        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40301);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        IssueRequestDto requestDto = _issueRequestGenerator.Clone().RuleFor(i => i.Title, "").Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40001);
        responseDto.ErrorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Create_EditorForEditorIdNotExist_ReturnsForbidden()
    {
        IssueRequestDto requestDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.EditorId, 1).Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40311);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ValidState_ReturnsOkWithListOfIssues()
    {
        var response = await _client.GetAsync(BasePath);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        var responseDto = await response.Content.ReadFromJsonAsync<List<IssueResponseDto>>();
        responseDto.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_IssueNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BasePath}/-1");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);


        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40401);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_IssueExist_ReturnsExistingResult()
    {
        Issue issue = _issueGenerator.Generate();
        _dbContext.Add(issue);
        await _dbContext.SaveChangesAsync();


        var response = await _client.GetAsync($"{BasePath}/{issue.Id}");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<IssueResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(issue,
            options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_IssueNotExist_ReturnsNotFound()
    {
        IssueRequestDto requestDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.Id, -1).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, requestDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40402);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_ValidData_ReturnsOkWithUpdatedIssue()
    {
        Issue createIssue = _issueGenerator.Generate();
        _dbContext.Add(createIssue);
        await _dbContext.SaveChangesAsync();

        IssueRequestDto updateDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.Id, createIssue.Id).Generate();
        

        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<IssueResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(updateDto,
            options => options.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task Update_NonUniqueValue_ReturnsForbiddenStatus()
    {
        Issue createIssue = _issueGenerator.Generate();
        _dbContext.Add(createIssue);
        await _dbContext.SaveChangesAsync();
        IssueRequestDto updateDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.Title, createIssue.Title).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40302);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_InvalidData_ReturnsBadRequest()
    {
        Issue createIssue = _issueGenerator.Generate();
        _dbContext.Add(createIssue);
        await _dbContext.SaveChangesAsync();

        IssueRequestDto updateDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.Id, createIssue.Id)
            .RuleFor(i => i.Title, "").Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40002);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_EditorForEditorIdNotExist_ReturnsForbidden()
    {
        Issue createIssue = _issueGenerator.Generate();
        _dbContext.Add(createIssue);
        await _dbContext.SaveChangesAsync();

        IssueRequestDto updateDto = _issueRequestGenerator.Clone()
            .RuleFor(i => i.Id, createIssue.Id)
            .RuleFor(i => i.EditorId, 1).Generate();


        var response = await _client.PutAsJsonAsync(BasePath, updateDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40312);
        responseDto.ErrorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Delete_IssueNotExist_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{BasePath}/-1");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40403);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_IssueExist_ReturnsNoContent()
    {
        Issue createIssue = _issueGenerator.Generate();
        _dbContext.Add(createIssue);
        await _dbContext.SaveChangesAsync();

        
        var response = await _client.DeleteAsync($"{BasePath}/{createIssue.Id}");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}