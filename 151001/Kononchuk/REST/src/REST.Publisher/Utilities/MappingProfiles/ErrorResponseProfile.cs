using AutoMapper;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Utilities.Exceptions;

namespace REST.Publisher.Utilities.MappingProfiles;

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
        CreateMap<UniqueConstraintException, ErrorResponseDto>()
            .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.ErrorCode, opt => opt.MapFrom(src => src.Code));
        CreateMap<ValidationException, ErrorResponseDto>()
            .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.ErrorCode, opt => opt.MapFrom(src => src.Code));
    }
}