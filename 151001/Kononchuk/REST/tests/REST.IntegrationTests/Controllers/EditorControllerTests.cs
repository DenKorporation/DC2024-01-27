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
public class EditorControllerTests(RestWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly AppDbContext _dbContext =
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    private readonly Func<Task> _resetDatabase = factory.ResetDatabase;
    private readonly HttpClient _client = factory.HttpClient;
    private const string BasePath = "editors";

    private readonly Faker<EditorRequestDto> _editorRequestGenerator = EditorGenerator.CreateEditorRequestDtoFaker();
    private readonly Faker<Editor> _editorGenerator = EditorGenerator.CreateEditorFaker();

    [Fact]
    public async Task Create_ValidData_ReturnsCreatedWithCreatedEditor()
    {
        EditorRequestDto requestDto = _editorRequestGenerator.Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);

        var responseDto = await response.Content.ReadFromJsonAsync<EditorResponseDto>();
        responseDto!.Id.Should().BePositive();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(requestDto,
            options => options.ExcludingMissingMembers()
                .Excluding(e => e.Id)
                .Excluding(e => e.Password));
    }
    
    [Fact]
    public async Task Create_NonUniqueValue_ReturnsForbiddenStatus()
    {
        Editor createEditor = _editorGenerator.Generate();
        _dbContext.Add(createEditor);
        await _dbContext.SaveChangesAsync();
        EditorRequestDto requestDto = _editorRequestGenerator.Clone()
            .RuleFor(e => e.Login, createEditor.Login).Generate();

        
        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40301);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        EditorRequestDto requestDto = _editorRequestGenerator.Clone().RuleFor(e => e.Password, "").Generate();


        var response = await _client.PostAsJsonAsync(BasePath, requestDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40001);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ValidState_ReturnsOkWithListOfEditors()
    {
        var response = await _client.GetAsync(BasePath);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        var responseDto = await response.Content.ReadFromJsonAsync<List<EditorResponseDto>>();
        responseDto.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_EditorNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BasePath}/-1");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);


        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40401);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_EditorExist_ReturnsExistingResult()
    {
        Editor editor = _editorGenerator.Generate();
        _dbContext.Add(editor);
        await _dbContext.SaveChangesAsync();


        var response = await _client.GetAsync($"{BasePath}/{editor.Id}");


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<EditorResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(editor,
            options => options.ExcludingMissingMembers()
                .Excluding(e => e.Password));
    }

    [Fact]
    public async Task Update_EditorNotExist_ReturnsNotFound()
    {
        EditorRequestDto requestDto = _editorRequestGenerator.Clone()
            .RuleFor(e => e.Id, -1).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, requestDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40402);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_ValidData_ReturnsOkWithUpdatedEditor()
    {
        Editor createEditor = _editorGenerator.Generate();
        _dbContext.Add(createEditor);
        await _dbContext.SaveChangesAsync();

        EditorRequestDto updateDto = _editorRequestGenerator.Clone()
            .RuleFor(e => e.Id, createEditor.Id).Generate();
        

        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<EditorResponseDto>();
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(updateDto,
            options => options.ExcludingMissingMembers()
                .Excluding(e => e.Password));
    }
    
    [Fact]
    public async Task Update_NonUniqueValue_ReturnsForbiddenStatus()
    {
        Editor createEditor = _editorGenerator.Generate();
        _dbContext.Add(createEditor);
        await _dbContext.SaveChangesAsync();
        EditorRequestDto updateDto = _editorRequestGenerator.Clone()
            .RuleFor(e => e.Login, createEditor.Login).Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status403Forbidden);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40302);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_InvalidData_ReturnsBadRequest()
    {
        Editor createEditor = _editorGenerator.Generate();
        _dbContext.Add(createEditor);
        await _dbContext.SaveChangesAsync();

        EditorRequestDto updateDto = _editorRequestGenerator.Clone()
            .RuleFor(e => e.Id, createEditor.Id)
            .RuleFor(e => e.Password, "").Generate();

        
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40002);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_EditorNotExist_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{BasePath}/-1");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        responseDto!.ErrorCode.Should().Be(40403);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_EditorExist_ReturnsNoContent()
    {
        Editor createEditor = _editorGenerator.Generate();
        _dbContext.Add(createEditor);
        await _dbContext.SaveChangesAsync();

        
        var response = await _client.DeleteAsync($"{BasePath}/{createEditor.Id}");

        
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}