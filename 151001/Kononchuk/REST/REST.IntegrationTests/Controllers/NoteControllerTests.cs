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
public class NoteControllerTests(RestWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly AppDbContext _dbContext =
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    private readonly Func<Task> _resetDatabase = factory.ResetDatabase;
    private readonly HttpClient _client = factory.HttpClient;
    private const string BasePath = "notes";

    private readonly Faker<NoteRequestDto> _noteRequestGenerator = NoteGenerator.CreateNoteRequestDtoFaker();
    private readonly Faker<Note> _noteGenerator = NoteGenerator.CreateNoteFaker();

    private Issue PrepareDb()
    {
        Issue issue = IssueGenerator.CreateIssueFaker().Generate();
        _dbContext.Add(issue);
        _dbContext.SaveChanges();

        return issue;
    }

    [Fact]
    public async Task Create_ValidData_ReturnsCreatedWithCreatedNote()
    {
        Issue issue = PrepareDb();
        NoteRequestDto requestDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id).Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);

        var responseDto = await response.Content.ReadFromJsonAsync<NoteResponseDto>();
        responseDto!.Id.Should().BePositive();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(requestDto,
            options => options.ExcludingMissingMembers()
                .Excluding(n => n.Id));
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        Issue issue = PrepareDb();
        NoteRequestDto requestDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id)
            .RuleFor(n => n.Content, "").Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40001);
        responseDto.ErrorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Create_IssueForIssueIdNotExist_ReturnsForbidden()
    {
        NoteRequestDto requestDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.IssueId, 1).Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40311);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ValidState_ReturnsOkWithListOfNotes()
    {
        var response = await _client.GetAsync(BasePath);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        var responseDto = await response.Content.ReadFromJsonAsync<List<NoteResponseDto>>();
        responseDto.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_NoteNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BasePath}/-1");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);


        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40401);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_NoteExist_ReturnsExistingResult()
    {
        Issue issue = PrepareDb();
        Note note = _noteGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id).Generate();
        _dbContext.Add(note);
        await _dbContext.SaveChangesAsync();


        var response = await _client.GetAsync($"{BasePath}/{note.Id}");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<NoteResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(note,
            options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_NoteNotExist_ReturnsNotFound()
    {
        NoteRequestDto requestDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, -1).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, requestDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40402);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_ValidData_ReturnsOkWithUpdatedNote()
    {
        Issue issue = PrepareDb();
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id).Generate();
        _dbContext.Add(createNote);
        await _dbContext.SaveChangesAsync();

        NoteRequestDto updateDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, createNote.Id)
            .RuleFor(n => n.IssueId, issue.Id).Generate();
        

        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<NoteResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(updateDto,
            options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_InvalidData_ReturnsBadRequest()
    {
        Issue issue = PrepareDb();
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id).Generate();
        _dbContext.Add(createNote);
        await _dbContext.SaveChangesAsync();

        NoteRequestDto updateDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, createNote.Id)
            .RuleFor(n => n.Content, "").Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40002);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_IssueForIssueIdNotExist_ReturnsForbidden()
    {
        Issue issue = PrepareDb();
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id).Generate();
        _dbContext.Add(createNote);
        await _dbContext.SaveChangesAsync();

        NoteRequestDto updateDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, createNote.Id)
            .RuleFor(n => n.IssueId, issue.Id + 1).Generate();


        var response = await _client.PutAsJsonAsync(BasePath, updateDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40312);
        responseDto.ErrorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Delete_NoteNotExist_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{BasePath}/-1");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40403);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_NoteExist_ReturnsNoContent()
    {
        Issue issue = PrepareDb();
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.IssueId, issue.Id).Generate();
        _dbContext.Add(createNote);
        await _dbContext.SaveChangesAsync();

        
        var response = await _client.DeleteAsync($"{BasePath}/{createNote.Id}");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}