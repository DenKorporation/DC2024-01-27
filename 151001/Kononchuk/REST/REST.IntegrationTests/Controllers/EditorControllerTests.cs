using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using REST.IntegrationTests.Helpers;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;

namespace REST.IntegrationTests.Controllers;

//TODO review this after migrating to the database
public class EditorControllerTests(RestWebApplicationFactory factory) : IClassFixture<RestWebApplicationFactory>
{
    // private readonly HttpClient _client = factory.CreateClient();
    // private static readonly string BaseUrl = "api/v1.0/editors";
    //
    // [Fact]
    // public async Task Create_ValidData_ReturnsOkWithCreatedEditor()
    // {
    //     EditorRequestDto requestDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_create_login", Password = "qwerty12", FirstName = "test_create_firstname",
    //         LastName = "test_create_lastname"
    //     };
    //
    //     var response = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<EditorResponseDto>();
    //     response.Should().NotBeNull();
    //     responseDto!.Id.Should().BePositive();
    //     responseDto.FirstName.Should().Be(requestDto.FirstName);
    //     responseDto.LastName.Should().Be(requestDto.LastName);
    //     responseDto.Login.Should().Be(requestDto.Login);
    // }
    //
    // [Fact]
    // public async Task Create_InvalidData_ReturnsBadRequest()
    // {
    //     EditorRequestDto requestDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_create_login", Password = "", FirstName = "test_create_firstname",
    //         LastName = "test_create_lastname"
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
    // public async Task GetAll_ValidState_ReturnsOkWithListOfEditors()
    // {
    //     var response = await _client.GetAsync(BaseUrl);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    // }
    //
    // [Fact]
    // public async Task GetById_EditorNotExist_ReturnsNotFound()
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
    // public async Task GetById_EditorExist_ReturnsExistingResult()
    // {
    //     EditorRequestDto requestDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_get_login", Password = "qwerty12", FirstName = "test_get_firstname",
    //         LastName = "test_get_lastname"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //     var createdEditor = await postResponse.Content.ReadFromJsonAsync<EditorResponseDto>();
    //
    //     var response = await _client.GetAsync(BaseUrl + $"/{createdEditor!.Id}");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<EditorResponseDto>();
    //
    //     responseDto!.FirstName.Should().Be(requestDto.FirstName);
    //     responseDto.LastName.Should().Be(requestDto.LastName);
    //     responseDto.Login.Should().Be(requestDto.Login);
    // }
    //
    // [Fact]
    // public async Task Update_EditorNotExist_ReturnsNotFound()
    // {
    //     EditorRequestDto requestDto = new EditorRequestDto
    //     {
    //         Id = -1, Login = "test_editor_update_login", Password = "qwerty12", FirstName = "test_update_firstname",
    //         LastName = "test_update_lastname"
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
    // public async Task Update_ValidData_ReturnsOkWithUpdatedEditor()
    // {
    //     EditorRequestDto createDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_create_login", Password = "qwerty12", FirstName = "test_create_firstname",
    //         LastName = "test_create_lastname"
    //     };
    //
    //     EditorRequestDto updateDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_update_login", Password = "qwerty12", FirstName = "test_update_firstname",
    //         LastName = "test_update_lastname"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
    //     var createdEditor = await postResponse.Content.ReadFromJsonAsync<EditorResponseDto>();
    //     updateDto.Id = createdEditor!.Id;
    //
    //     var response = await _client.PutAsJsonAsync(BaseUrl, updateDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<EditorResponseDto>();
    //     response.Should().NotBeNull();
    //     responseDto!.Id.Should().BePositive();
    //     responseDto.FirstName.Should().Be(updateDto.FirstName);
    //     responseDto.LastName.Should().Be(updateDto.LastName);
    //     responseDto.Login.Should().Be(updateDto.Login);
    // }
    //
    // [Fact]
    // public async Task Update_InvalidData_ReturnsBadRequest()
    // {
    //     EditorRequestDto createDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_create_login", Password = "qwerty12", FirstName = "test_create_firstname",
    //         LastName = "test_create_lastname"
    //     };
    //
    //     EditorRequestDto updateDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_update_login", Password = "", FirstName = "test_update_firstname",
    //         LastName = "test_update_lastname"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
    //     var createdEditor = await postResponse.Content.ReadFromJsonAsync<EditorResponseDto>();
    //     updateDto.Id = createdEditor!.Id;
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
    // public async Task Delete_EditorNotExist_ReturnsNotFound()
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
    // public async Task Delete_EditorExist_ReturnsNoContent()
    // {
    //     EditorRequestDto requestDto = new EditorRequestDto
    //     {
    //         Login = "test_editor_delete_login", Password = "qwerty12", FirstName = "test_delete_firstname",
    //         LastName = "test_delete_lastname"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //     var createdEditor = await postResponse.Content.ReadFromJsonAsync<EditorResponseDto>();
    //
    //     var response = await _client.DeleteAsync(BaseUrl + $"/{createdEditor!.Id}");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    // }
}