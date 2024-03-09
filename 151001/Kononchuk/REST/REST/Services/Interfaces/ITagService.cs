using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;

namespace REST.Services.Interfaces;

public interface ITagService : IService<TagRequestDto, TagResponseDto>
{
}