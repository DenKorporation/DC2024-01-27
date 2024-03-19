using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using REST.IntegrationTests.Helpers;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;

namespace REST.IntegrationTests.Controllers;

//TODO review this after migrating to the database
public class IssueControllerTests(RestWebApplicationFactory factory) : IClassFixture<RestWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static readonly string BaseUrl = "api/v1.0/issues";

    [Fact]
    public async Task Create_ValidData_ReturnsOkWithCreatedIssue()
    {
        IssueRequestDto requestDto = new IssueRequestDto
        {
            Content = "test_create_content", Title = "test_create_title", EditorId = -1
        };

        var response = await _client.PostAsJsonAsync(BaseUrl, requestDto);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);

        var responseDto = await response.Content.ReadFromJsonAsync<IssueResponseDto>();
        response.Should().NotBeNull();
        responseDto!.Id.Should().BePositive();
        responseDto.Content.Should().Be(requestDto.Content);
        responseDto.Title.Should().Be(requestDto.Title);
        responseDto.EditorId.Should().Be(requestDto.EditorId);
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        IssueRequestDto requestDto = new IssueRequestDto
        {
            Content = "test_create_content", Title = "", EditorId = -1
        };

        var response = await _client.PostAsJsonAsync(BaseUrl, requestDto);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40001);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ValidState_ReturnsOkWithListOfIssues()
    {
        var response = await _client.GetAsync(BaseUrl);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetById_IssueNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync(BaseUrl + "/-1");

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40401);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_IssueExist_ReturnsExistingResult()
    {
        IssueRequestDto requestDto = new IssueRequestDto
        {
            Content = "test_get_content", Title = "test_get_title", EditorId = -1
        };

        var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
        var createdIssue = await postResponse.Content.ReadFromJsonAsync<IssueResponseDto>();

        var response = await _client.GetAsync(BaseUrl + $"/{createdIssue!.Id}");

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<IssueResponseDto>();

        responseDto!.Content.Should().Be(requestDto.Content);
        responseDto.Title.Should().Be(requestDto.Title);
        responseDto.EditorId.Should().Be(requestDto.EditorId);
    }

    [Fact]
    public async Task Update_IssueNotExist_ReturnsNotFound()
    {
        IssueRequestDto requestDto = new IssueRequestDto
        {
            Id = -1, Content = "test_update_content", Title = "test_update_title", EditorId = -1
        };

        var response = await _client.PutAsJsonAsync(BaseUrl, requestDto);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40402);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_ValidData_ReturnsOkWithUpdatedIssue()
    {
        IssueRequestDto createDto = new IssueRequestDto
        {
            Content = "test_create_content", Title = "test_create_title", EditorId = -1
        };

        IssueRequestDto updateDto = new IssueRequestDto
        {
            Content = "test_update_content", Title = "test_update_title", EditorId = -1
        };

        var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
        var createdIssue = await postResponse.Content.ReadFromJsonAsync<IssueResponseDto>();
        updateDto.Id = createdIssue!.Id;

        var response = await _client.PutAsJsonAsync(BaseUrl, updateDto);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        var responseDto = await response.Content.ReadFromJsonAsync<IssueResponseDto>();
        response.Should().NotBeNull();
        responseDto!.Id.Should().BePositive();
        responseDto.Content.Should().Be(updateDto.Content);
        responseDto.Title.Should().Be(updateDto.Title);
        responseDto.EditorId.Should().Be(updateDto.EditorId);
    }

    [Fact]
    public async Task Update_InvalidData_ReturnsBadRequest()
    {
        IssueRequestDto createDto = new IssueRequestDto
        {
            Content = "test_create_content", Title = "test_create_title", EditorId = -1
        };

        IssueRequestDto updateDto = new IssueRequestDto
        {
            Content = "test_update_content", Title = "", EditorId = -1
        };

        var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
        var createdIssue = await postResponse.Content.ReadFromJsonAsync<IssueResponseDto>();
        updateDto.Id = createdIssue!.Id;

        var response = await _client.PutAsJsonAsync(BaseUrl, updateDto);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40002);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_IssueNotExist_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync(BaseUrl + "/-1");

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        responseDto!.ErrorCode.Should().Be(40403);
        responseDto.ErrorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_IssueExist_ReturnsNoContent()
    {
        IssueRequestDto requestDto = new IssueRequestDto
        {
            Content = "test_delete_content", Title = "test_delete_title", EditorId = -1
        };

        var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
        var createdIssue = await postResponse.Content.ReadFromJsonAsync<IssueResponseDto>();

        var response = await _client.DeleteAsync(BaseUrl + $"/{createdIssue!.Id}");

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    }
}