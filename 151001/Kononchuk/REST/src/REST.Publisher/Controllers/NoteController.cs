using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Services.Interfaces;
using RestSharp;

namespace REST.Publisher.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/notes")]
public class NoteController(IRestClient client) : Controller
{
    private const string BasePath = "notes";
    
    [HttpPost]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create()
    {
        var request = new RestRequest(BasePath);

        foreach (var header in HttpContext.Request.Headers)
        {
            request.AddHeader(header.Key, header.Value.ToString());
        }
        
        var stream = new StreamReader(HttpContext.Request.Body);
        var body = await stream.ReadToEndAsync();

        request.AddStringBody(body, DataFormat.Json);

        var response = await client.PostAsync(request);

        return StatusCode((int)response.StatusCode, response.Content);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<NoteResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var request = new RestRequest(BasePath);

        foreach (var header in HttpContext.Request.Headers)
        {
            request.AddHeader(header.Key, header.Value.ToString());
        }

        var response = await client.GetAsync(request);

        return StatusCode((int)response.StatusCode, response.Content);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var request = new RestRequest(BasePath + $"/{id}");

        foreach (var header in HttpContext.Request.Headers)
        {
            request.AddHeader(header.Key, header.Value.ToString());
        }

        var response = await client.GetAsync(request);

        return StatusCode((int)response.StatusCode, response.Content);
    }

    [HttpPut]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update()
    {
        var request = new RestRequest(BasePath);

        foreach (var header in HttpContext.Request.Headers)
        {
            request.AddHeader(header.Key, header.Value.ToString());
        }
        
        var stream = new StreamReader(HttpContext.Request.Body);
        var body = await stream.ReadToEndAsync();

        request.AddStringBody(body, DataFormat.Json);

        var response = await client.PutAsync(request);

        return StatusCode((int)response.StatusCode, response.Content);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var request = new RestRequest(BasePath + $"/{id}");

        foreach (var header in HttpContext.Request.Headers)
        {
            request.AddHeader(header.Key, header.Value.ToString());
        }

        var response = await client.DeleteAsync(request);

        return StatusCode((int)response.StatusCode, response.Content);
    }
}