using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Services.Interfaces;

namespace REST.Publisher.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/tags")]
public class TagController(ITagService tagService) : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(TagResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] TagRequestDto dto)
    {
        var tag = await tagService.CreateAsync(dto);

        return CreatedAtAction(null, tag);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TagResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var tags = await tagService.GetAllAsync();

        return Ok(tags);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TagResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var tag = await tagService.GetByIdAsync(id);

        return Ok(tag);
    }

    [HttpPut]
    [ProducesResponseType(typeof(TagResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update([FromBody] TagRequestDto dto)
    {
        var tag = await tagService.UpdateAsync(dto.Id, dto);

        return Ok(tag);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        await tagService.DeleteAsync(id);
        
        return NoContent();
    }
}