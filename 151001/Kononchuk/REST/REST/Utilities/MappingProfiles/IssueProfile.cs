using AutoMapper;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;

namespace REST.Utilities.MappingProfiles;

public class IssueProfile : Profile
{
    public IssueProfile()
    {
        CreateMap<Issue, IssueResponseDto>();
        CreateMap<IssueRequestDto, Issue>();
    }
}