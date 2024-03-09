using AutoMapper;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;

namespace REST.Utilities.MappingProfiles;

public class NoteProfile: Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteResponseDto>();
        CreateMap<NoteRequestDto, Note>();
    }
}