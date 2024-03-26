using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using REST.IntegrationTests.Helpers;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;

namespace REST.IntegrationTests.Controllers;

//TODO review this after migrating to the database
public class TagControllerTests(RestWebApplicationFactory factory) : IClassFixture<RestWebApplicationFactory>
{
    // private readonly HttpClient _client = factory.CreateClient();
    // private static readonly string BaseUrl = "api/v1.0/tags";
    //
    // [Fact]
    // public async Task Create_ValidData_ReturnsOkWithCreatedTag()
    // {
    //     TagRequestDto requestDto = new TagRequestDto
    //     {
    //         Name = "test_create_name"
    //     };
    //
    //     var response = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<TagResponseDto>();
    //     response.Should().NotBeNull();
    //     responseDto!.Id.Should().BePositive();
    //     responseDto.Name.Should().Be(requestDto.Name);
    // }
    //
    // [Fact]
    // public async Task Create_InvalidData_ReturnsBadRequest()
    // {
    //     TagRequestDto requestDto = new TagRequestDto
    //     {
    //         Name = ""
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
    // public async Task GetAll_ValidState_ReturnsOkWithListOfTags()
    // {
    //     var response = await _client.GetAsync(BaseUrl);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    // }
    //
    // [Fact]
    // public async Task GetById_TagNotExist_ReturnsNotFound()
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
    // public async Task GetById_TagExist_ReturnsExistingResult()
    // {
    //     TagRequestDto requestDto = new TagRequestDto
    //     {
    //         Name = "test_get_name"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //     var createdTag = await postResponse.Content.ReadFromJsonAsync<TagResponseDto>();
    //
    //     var response = await _client.GetAsync(BaseUrl + $"/{createdTag!.Id}");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<TagResponseDto>();
    //
    //     responseDto!.Name.Should().Be(requestDto.Name);
    // }
    //
    // [Fact]
    // public async Task Update_TagNotExist_ReturnsNotFound()
    // {
    //     TagRequestDto requestDto = new TagRequestDto
    //     {
    //         Id = -1, Name = "test_update_name"
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
    // public async Task Update_ValidData_ReturnsOkWithUpdatedTag()
    // {
    //     TagRequestDto createDto = new TagRequestDto
    //     {
    //         Name = "test_create_name"
    //     };
    //
    //     TagRequestDto updateDto = new TagRequestDto
    //     {
    //         Name = "test_update_name"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
    //     var createdTag = await postResponse.Content.ReadFromJsonAsync<TagResponseDto>();
    //     updateDto.Id = createdTag!.Id;
    //
    //     var response = await _client.PutAsJsonAsync(BaseUrl, updateDto);
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    //
    //     var responseDto = await response.Content.ReadFromJsonAsync<TagResponseDto>();
    //     response.Should().NotBeNull();
    //     responseDto!.Id.Should().BePositive();
    //     responseDto.Name.Should().Be(updateDto.Name);
    // }
    //
    // [Fact]
    // public async Task Update_InvalidData_ReturnsBadRequest()
    // {
    //     TagRequestDto createDto = new TagRequestDto
    //     {
    //         Name = "test_create_name"
    //     };
    //
    //     TagRequestDto updateDto = new TagRequestDto
    //     {
    //         Name = ""
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
    //     var createdTag = await postResponse.Content.ReadFromJsonAsync<TagResponseDto>();
    //     updateDto.Id = createdTag!.Id;
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
    // public async Task Delete_TagNotExist_ReturnsNotFound()
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
    // public async Task Delete_TagExist_ReturnsNoContent()
    // {
    //     TagRequestDto requestDto = new TagRequestDto
    //     {
    //         Name = "test_delete_name"
    //     };
    //
    //     var postResponse = await _client.PostAsJsonAsync(BaseUrl, requestDto);
    //     var createdTag = await postResponse.Content.ReadFromJsonAsync<TagResponseDto>();
    //
    //     var response = await _client.DeleteAsync(BaseUrl + $"/{createdTag!.Id}");
    //
    //     response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
    // }
}