using REST.Discussion.Models.DTOs.Request;
using REST.Discussion.Models.DTOs.Response;
using REST.Discussion.Models.Entities;

namespace REST.Discussion.Services.Interfaces;

public interface INoteService: IService<NoteRequestDto, NoteResponseDto, NoteKey>
{
}