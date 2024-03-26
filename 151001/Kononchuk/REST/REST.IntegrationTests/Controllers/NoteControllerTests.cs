using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using REST.IntegrationTests.Helpers;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;

namespace REST.IntegrationTests.Controllers;

//TODO review this after migrating to the database
public class NoteControllerTests(RestWebApplicationFactory factory) : IClassFixture<RestWebApplicationFactory>
{
    // private readonly HttpClient _client = factory.CreateClient();
    // private static readonly string BaseUrl = "api/v1.0/notes";
    //
    // [Fact]
    // public async Task Create_ValidData_ReturnsOkWithCreatedNote()
    // {
    //     NoteRequestDto requestDto = new NoteRequestDto
    //     {
    //         Content = "test_create_content"
    //     };
    //
    //     var response = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<NoteResponseDto>();
    //     response.Should().NotBeNull();
    //     responseDto!.Id.Should().BePositive();
    //     responseDto.Content.Should().Be(requestDto.Content);
    //     responseDto.IssueId.Should().Be(requestDto.IssueId);
    // }
    //
    // [Fact]
    // public async Task Create_InvalidData_ReturnsBadRequest()
    // {
    //     NoteRequestDto requestDto = new NoteRequestDto
    //     {
    //         Content = ""
    //     };
    //
    //     var response = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
    //
    //     responseDto!.ErrorCode.Should().Be(40001);
    //     responseDto.ErrorMessage.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task GetAll_ValidState_ReturnsOkWithListOfNotes()
    // {
    //     var response = await _client.GetAsync(BaseUrl);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    // }
    //
    // [Fact]
    // public async Task GetById_NoteNotExist_ReturnsNotFound()
    // {
    //     var response = await _client.GetAsync(BaseUrl + "/-1");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
    //
    //     responseDto!.ErrorCode.Should().Be(40401);
    //     responseDto.ErrorMessage.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task GetById_NoteExist_ReturnsExistingResult()
    // {
    //     NoteRequestDto requestDto = new NoteRequestDto
    //     {
    //         Content = "test_get_content"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //     var createdNote = await postResponse.Content.ReadFromJsonAsync<NoteResponseDto>();
    //
    //     var response = await _client.GetAsync(BaseUrl + $"/{createdNote!.Id}");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<NoteResponseDto>();
    //
    //     responseDto!.Content.Should().Be(requestDto.Content);
    //     responseDto.IssueId.Should().Be(requestDto.IssueId);
    // }
    //
    // [Fact]
    // public async Task Update_NoteNotExist_ReturnsNotFound()
    // {
    //     NoteRequestDto requestDto = new NoteRequestDto
    //     {
    //         Id = -1, Content = "test_update_content"
    //     };
    //
    //     var response = await _client.PutAsJsonAsync(BaseUrl, requestDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
    //
    //     responseDto!.ErrorCode.Should().Be(40402);
    //     responseDto.ErrorMessage.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task Update_ValidData_ReturnsOkWithUpdatedNote()
    // {
    //     NoteRequestDto createDto = new NoteRequestDto
    //     {
    //         Content = "test_create_content"
    //     };
    //
    //     NoteRequestDto updateDto = new NoteRequestDto
    //     {
    //         Content = "test_update_content"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
    //     var createdNote = await postResponse.Content.ReadFromJsonAsync<NoteResponseDto>();
    //     updateDto.Id = createdNote!.Id;
    //
    //     var response = await _client.PutAsJsonAsync(BaseUrl, updateDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<NoteResponseDto>();
    //     response.Should().NotBeNull();
    //     responseDto!.Id.Should().BePositive();
    //     responseDto.Content.Should().Be(updateDto.Content);
    //     responseDto.IssueId.Should().Be(updateDto.IssueId);
    // }
    //
    // [Fact]
    // public async Task Update_InvalidData_ReturnsBadRequest()
    // {
    //     NoteRequestDto createDto = new NoteRequestDto
    //     {
    //         Content = "test_create_content"
    //     };
    //
    //     NoteRequestDto updateDto = new NoteRequestDto
    //     {
    //         Content = ""
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
    //     var createdNote = await postResponse.Content.ReadFromJsonAsync<NoteResponseDto>();
    //     updateDto.Id = createdNote!.Id;
    //
    //     var response = await _client.PutAsJsonAsync(BaseUrl, updateDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
    //
    //     responseDto!.ErrorCode.Should().Be(40002);
    //     responseDto.ErrorMessage.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task Delete_NoteNotExist_ReturnsNotFound()
    // {
    //     var response = await _client.DeleteAsync(BaseUrl + "/-1");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
    //
    //     responseDto!.ErrorCode.Should().Be(40403);
    //     responseDto.ErrorMessage.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task Delete_NoteExist_ReturnsNoContent()
    // {
    //     NoteRequestDto requestDto = new NoteRequestDto
    //     {
    //         Content = "test_delete_content"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //     var createdNote = await postResponse.Content.ReadFromJsonAsync<NoteResponseDto>();
    //
    //     var response = await _client.DeleteAsync(BaseUrl + $"/{createdNote!.Id}");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    // }
}