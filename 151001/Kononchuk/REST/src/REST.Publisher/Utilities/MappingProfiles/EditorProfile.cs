using AutoMapper;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Models.Entities;

namespace REST.Publisher.Utilities.MappingProfiles;

public class EditorProfile : Profile
{
    public EditorProfile()
    {
        CreateMap<Editor, EditorResponseDto>();
        CreateMap<EditorRequestDto, Editor>();
    }
}