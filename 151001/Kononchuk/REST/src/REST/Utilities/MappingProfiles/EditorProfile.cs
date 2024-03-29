using AutoMapper;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;

namespace REST.Utilities.MappingProfiles;

public class EditorProfile : Profile
{
    public EditorProfile()
    {
        CreateMap<Editor, EditorResponseDto>();
        CreateMap<EditorRequestDto, Editor>();
    }
}