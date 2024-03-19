using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Services.Interfaces;

namespace REST.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/notes")]
public class NoteController(INoteService noteService) : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] NoteRequestDto dto)
    {
        var note = await noteService.CreateAsync(dto);

        return CreatedAtAction(null, note);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<NoteResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var notes = await noteService.GetAllAsync();

        return Ok(notes);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var note = await noteService.GetByIdAsync(id);

        return Ok(note);
    }

    [HttpPut]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update([FromBody] NoteRequestDto dto)
    {
        var note = await noteService.UpdateAsync(dto.Id, dto);

        return Ok(note);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        await noteService.DeleteAsync(id);
        return NoContent();
    }
}