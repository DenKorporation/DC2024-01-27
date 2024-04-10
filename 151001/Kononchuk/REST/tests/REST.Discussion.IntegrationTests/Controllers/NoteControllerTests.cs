using System.Net;
using Bogus;
using Cassandra.Mapping;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using REST.Discussion.Data;
using REST.Discussion.IntegrationTests.DataGenerators;
using REST.Discussion.IntegrationTests.Fixtures;
using REST.Discussion.Models.DTOs.Request;
using REST.Discussion.Models.DTOs.Response;
using REST.Discussion.Models.Entities;
using RestSharp;

namespace REST.Discussion.IntegrationTests.Controllers;

[Collection("Controller Collection")]
public class NoteControllerTests(RestWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly Mapper _dbContext = new(
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<CassandraContext>().Session);

    private readonly Func<Task> _resetDatabase = factory.ResetDatabase;
    private readonly RestClient _client = new(factory.HttpClient);
    private const string BasePath = "notes";

    private readonly Faker<NoteRequestDto> _noteRequestGenerator = NoteGenerator.CreateNoteRequestDtoFaker();
    private readonly Faker<Note> _noteGenerator = NoteGenerator.CreateNoteFaker();

    [Fact]
    public async Task Create_ValidData_ReturnsCreatedWithCreatedNote()
    {
        NoteRequestDto requestDto = _noteRequestGenerator.Generate();

        var request = new RestRequest(BasePath);
        request.AddHeader("Accept-Language", "ru");
        request.AddJsonBody(requestDto);


        var response = await _client.PostAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<NoteResponseDto>(response.Content!);

        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().BePositive();
        responseDto.Should().BeEquivalentTo(requestDto,
            options => options.ExcludingMissingMembers()
                .Excluding(n => n.Id));
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        NoteRequestDto requestDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Content, "").Generate();


        var request = new RestRequest(BasePath, Method.Post);
        request.AddHeader("Accept-Language", "ru");
        request.AddJsonBody(requestDto);


        var response = await _client.ExecuteAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response.Content!);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorCode.Should().Be(40001);
        responseDto.ErrorMessage.Should().NotBeNull();
    }


    [Fact]
    public async Task GetAll_ValidState_ReturnsOkWithListOfNotes()
    {
        var request = new RestRequest(BasePath);
        var response = await _client.GetAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Content.Should().NotBeNull();
        var responseDto = JsonConvert.DeserializeObject<List<NoteResponseDto>>(response.Content!);
        responseDto.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_NoteNotExist_ReturnsNotFound()
    {
        var request = new RestRequest($"{BasePath}/-1");
        var response = await _client.GetAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response.Content!);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorCode.Should().Be(40401);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_NoteExist_ReturnsExistingResult()
    {
        Note note = _noteGenerator.Clone().RuleFor(n => n.Country, "ru").Generate();
        await _dbContext.InsertAsync(note);

        var request = new RestRequest($"{BasePath}/{note.Id}");


        var response = await _client.GetAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<NoteResponseDto>(response.Content!);
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(note,
            options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_NoteNotExist_ReturnsNotFound()
    {
        NoteRequestDto requestDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, -1).Generate();

        var request = new RestRequest(BasePath);
        request.AddHeader("Accept-Language", "ru");
        request.AddJsonBody(requestDto);


        var response = await _client.PutAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response.Content!);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorCode.Should().Be(40402);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_ValidData_ReturnsOkWithUpdatedNote()
    {
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.Country, "ru").Generate();
        await _dbContext.InsertAsync(createNote);

        NoteRequestDto updateDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, createNote.Id).Generate();
        
        var request = new RestRequest(BasePath);
        request.AddHeader("Accept-Language", "ru");
        request.AddJsonBody(updateDto);
        
        
        var response = await _client.PutAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<NoteResponseDto>(response.Content!);
        responseDto.Should().NotBeNull().Should().BeEquivalentTo(updateDto,
            options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_InvalidData_ReturnsBadRequest()
    {
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.Country, "ru").Generate();
        await _dbContext.InsertAsync(createNote);

        NoteRequestDto updateDto = _noteRequestGenerator.Clone()
            .RuleFor(n => n.Id, createNote.Id)
            .RuleFor(n => n.Content, "").Generate();

        var request = new RestRequest(BasePath, Method.Put);
        request.AddHeader("Accept-Language", "ru");
        request.AddJsonBody(updateDto);
        

        var response = await _client.ExecuteAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);
        response.Content.Should().NotBeNull();

        var responseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response.Content!);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorCode.Should().Be(40002);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_NoteNotExist_ReturnsNotFound()
    {
        var request = new RestRequest($"{BasePath}/-1");
        
        
        var response = await _client.DeleteAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        response.Content.Should().NotBeNull();
        
        var responseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response.Content!);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorCode.Should().Be(40403);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_NoteExist_ReturnsNoContent()
    {
        Note createNote = _noteGenerator.Clone()
            .RuleFor(n => n.Country, "ru").Generate();
        await _dbContext.InsertAsync(createNote);

        var request = new RestRequest($"{BasePath}/{createNote.Id}");
        

        var response = await _client.DeleteAsync(request);


        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}