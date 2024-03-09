using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Services.Interfaces;

namespace REST.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/editors")]
public class EditorController(IEditorService editorService) : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(EditorResponseDto), (int)HttpStatusCode.Created)]
    public ActionResult  Create([FromBody] EditorRequestDto dto)
    {
        // TODO: add a check for null and send the appropriate status code
        var editor = editorService.Create(dto);

        return CreatedAtAction(null, editor);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<EditorResponseDto>), (int)HttpStatusCode.OK)]
    public ActionResult GetAll()
    {
        var editors = editorService.GetAll();

        return Ok(editors);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EditorResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult GetById(long id)
    {
        var editor = editorService.GetById(id);

        if (editor is not null)
        {
            return Ok(editor);
        }

        return NotFound();
    }

    [HttpPut]
    [ProducesResponseType(typeof(EditorResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult Update([FromBody] EditorRequestDto dto)
    {
        var editor = editorService.Update(dto.Id, dto);

        if (editor is not null)
        {
            return Ok(editor);
        }

        return NotFound();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult Delete(long id)
    {
        if (editorService.Delete(id))
        {
            return NoContent();
        }

        return NotFound();
    }
}