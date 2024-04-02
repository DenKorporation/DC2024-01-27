using AutoMapper;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Models.Entities;

namespace REST.Publisher.Utilities.MappingProfiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagResponseDto>();
        CreateMap<TagRequestDto, Tag>();
    }
}