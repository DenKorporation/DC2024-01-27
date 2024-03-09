using AutoMapper;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;

namespace REST.Utilities.MappingProfiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagResponseDto>();
        CreateMap<TagRequestDto, Tag>();
    }
}