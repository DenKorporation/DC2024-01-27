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
    public ActionResult Create([FromBody] IssueRequestDto dto)
    {
        var issue = issueService.Create(dto);

        return CreatedAtAction(null, issue);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<IssueResponseDto>), (int)HttpStatusCode.OK)]
    public ActionResult GetAll()
    {
        var issues = issueService.GetAll();

        return Ok(issues);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(IssueResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult GetById(long id)
    {
        var issue = issueService.GetById(id);

        if (issue is not null)
        {
            return Ok(issue);
        }

        return NotFound();
    }

    [HttpPut]
    [ProducesResponseType(typeof(IssueResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult Update([FromBody] IssueRequestDto dto)
    {
        var issue = issueService.Update(dto.Id, dto);

        if (issue is not null)
        {
            return Ok(issue);
        }

        return NotFound();
    }
    // TODO update this for new logic
    // [HttpDelete("{id:long}")]
    // [ProducesResponseType((int)HttpStatusCode.NoContent)]
    // [ProducesResponseType((int)HttpStatusCode.NotFound)]
    // public ActionResult Delete(long id)
    // {
    //     if (issueService.Delete(id))
    //     {
    //         return NoContent();
    //     }
    //
    //     return NotFound();
    // }
}