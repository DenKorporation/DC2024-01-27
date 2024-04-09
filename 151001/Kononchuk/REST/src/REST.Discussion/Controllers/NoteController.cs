using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Discussion.Models.DTOs.Request;
using REST.Discussion.Models.DTOs.Response;
using REST.Discussion.Models.Entities;
using REST.Discussion.Services.Interfaces;

namespace REST.Discussion.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/notes")]
public class NoteController(INoteService noteService) : Controller
{
    private string GetCountryCode()
    {
        var languages = HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault();

        if (languages is null)
        {
            return "us";
        }

        var country = languages
            .Split(",").First()
            .Split(";").First()
            .Split("-").Last();
        return country.ToLower();
    }

    [HttpPost]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] NoteRequestDto dto)
    {
        dto.Country = GetCountryCode();
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
        var note = await noteService.GetByIdAsync(new NoteKey { Id = id });

        return Ok(note);
    }

    [HttpPut]
    [ProducesResponseType(typeof(NoteResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update([FromBody] NoteRequestDto dto)
    {
        dto.Country = GetCountryCode();
        var note = await noteService.UpdateAsync(dto);

        return Ok(note);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        await noteService.DeleteAsync(new NoteKey { Id = id });
        return NoContent();
    }
}