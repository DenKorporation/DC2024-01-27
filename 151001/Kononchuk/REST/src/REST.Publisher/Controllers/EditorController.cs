using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Services.Interfaces;

namespace REST.Publisher.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/editors")]
public class EditorController(IEditorService editorService) : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(EditorResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] EditorRequestDto dto)
    {
        var editor = await editorService.CreateAsync(dto);

        return CreatedAtAction(null, editor);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<EditorResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var editors = await editorService.GetAllAsync();

        return Ok(editors);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EditorResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var editor = await editorService.GetByIdAsync(id);

        return Ok(editor);
    }

    [HttpPut]
    [ProducesResponseType(typeof(EditorResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update([FromBody] EditorRequestDto dto)
    {
        var editor = await editorService.UpdateAsync(dto.Id, dto);

        return Ok(editor);
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        await editorService.DeleteAsync(id);
        return NoContent();
    }
}