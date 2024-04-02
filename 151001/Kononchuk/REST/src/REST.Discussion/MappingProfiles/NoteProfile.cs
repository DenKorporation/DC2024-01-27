﻿using AutoMapper;
using REST.Discussion.Models.DTOs.Request;
using REST.Discussion.Models.DTOs.Response;
using REST.Discussion.Models.Entities;

namespace REST.Discussion.MappingProfiles;

public class NoteProfile: Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteResponseDto>();
        CreateMap<NoteRequestDto, Note>();
    }
}