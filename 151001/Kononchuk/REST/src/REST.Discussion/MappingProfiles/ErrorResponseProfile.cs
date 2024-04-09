using AutoMapper;
using REST.Discussion.Exceptions;
using REST.Discussion.Models.DTOs.Response;

namespace REST.Discussion.MappingProfiles;

public class ErrorResponseProfile : Profile
{
    public ErrorResponseProfile()
    {
        CreateMap<AssociationException, ErrorResponseDto>()
            .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.ErrorCode, opt => opt.MapFrom(src => src.Code));
        CreateMap<ResourceNotFoundException, ErrorResponseDto>()
            .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.ErrorCode, opt => opt.MapFrom(src => src.Code));
        CreateMap<ValidationException, ErrorResponseDto>()
            .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.ErrorCode, opt => opt.MapFrom(src => src.Code));
    }
}