using REST.Discussion.Models.DTOs.Request;
using REST.Discussion.Models.DTOs.Response;

namespace REST.Discussion.Services.Interfaces;

public interface INoteService: IService<NoteRequestDto, NoteResponseDto>
{
}