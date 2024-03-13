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
    public ActionResult Create([FromBody] NoteRequestDto dto)
    {
        var note = noteService.Create(dto);

        return CreatedAtAction(null, note);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<NoteResponseDto>), (int)HttpStatusCode.OK)]
    public ActionResult GetAll()
    {
        var notes = noteService.GetAll();

        return Ok(notes);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult GetById(long id)
    {
        var note = noteService.GetById(id);

        if (note is not null)
        {
            return Ok(note);
        }

        return NotFound();
    }

    [HttpPut]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult Update([FromBody] NoteRequestDto dto)
    {
        var note = noteService.Update(dto.Id, dto);

        if (note is not null)
        {
            return Ok(note);
        }

        return NotFound();
    }

    // TODO update this for new logic
    // [HttpDelete("{id:long}")]
    // [ProducesResponseType((int)HttpStatusCode.NoContent)]
    // [ProducesResponseType((int)HttpStatusCode.NotFound)]
    // public ActionResult Delete(long id)
    // {
    //     if (noteService.Delete(id))
    //     {
    //         return NoContent();
    //     }
    //
    //     return NotFound();
    // }
}