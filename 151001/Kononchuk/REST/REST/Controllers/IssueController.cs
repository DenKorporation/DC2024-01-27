using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Services.Interfaces;

namespace REST.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/issues")]
public class IssueController(IIssueService issueService) : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(IssueResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] IssueRequestDto dto)
    {
        var issue = await issueService.CreateAsync(dto);

        return CreatedAtAction(null, issue);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<IssueResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var issues = await issueService.GetAllAsync();

        return Ok(issues);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(IssueResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var issue = await issueService.GetByIdAsync(id);

        return Ok(issue);
    }

    [HttpPut]
    [ProducesResponseType(typeof(IssueResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update([FromBody] IssueRequestDto dto)
    {
        var issue = await issueService.UpdateAsync(dto.Id, dto);
        return Ok(issue);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        await issueService.DeleteAsync(id);
        return NoContent();
    }
}