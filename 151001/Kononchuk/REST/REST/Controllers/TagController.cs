using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Services.Interfaces;

namespace REST.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/tags")]
public class TagController(ITagService tagService) : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(TagResponseDto), (int)HttpStatusCode.Created)]
    public ActionResult Create([FromBody] TagRequestDto dto)
    {
        var tag = tagService.Create(dto);

        return CreatedAtAction(null, tag);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TagResponseDto>), (int)HttpStatusCode.OK)]
    public ActionResult GetAll()
    {
        var tags = tagService.GetAll();

        return Ok(tags);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TagResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult GetById(long id)
    {
        var tag = tagService.GetById(id);

        if (tag is not null)
        {
            return Ok(tag);
        }

        return NotFound();
    }

    [HttpPut]
    [ProducesResponseType(typeof(TagResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult Update([FromBody] TagRequestDto dto)
    {
        var tag = tagService.Update(dto.Id, dto);

        if (tag is not null)
        {
            return Ok(tag);
        }

        return NotFound();
    }

    // TODO update this for new logic
    // [HttpDelete("{id:long}")]
    // [ProducesResponseType((int)HttpStatusCode.NoContent)]
    // [ProducesResponseType((int)HttpStatusCode.NotFound)]
    // public ActionResult Delete(long id)
    // {
    //     if (tagService.Delete(id))
    //     {
    //         return NoContent();
    //     }
    //
    //     return NotFound();
    // }
}