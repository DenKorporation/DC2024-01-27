using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;

namespace REST.Services.Implementations;

public class NoteService(
    IMapper mapper,
    INoteRepository<long> noteRepository,
    IIssueRepository<long> issueRepository,
    AbstractValidator<Note> validator) : INoteService
{
    public NoteResponseDto? Create(NoteRequestDto dto)
    {
        var note = mapper.Map<Note>(dto);

        var validationResult = validator.Validate(note);

        if (validationResult.IsValid)
        {
            if (note.IssueId == -1 || issueRepository.Exist(note.IssueId))
            {
                var createdNote = noteRepository.Add(note);

                if (createdNote is not null)
                {
                    return mapper.Map<NoteResponseDto>(createdNote);
                }
            }
        }

        return null;
    }

    public NoteResponseDto? GetById(long id)
    {
        var foundNote = noteRepository.GetById(id);

        if (foundNote is not null)
        {
            return mapper.Map<NoteResponseDto>(foundNote);
        }

        return null;
    }

    public List<NoteResponseDto> GetAll()
    {
        return noteRepository.GetAll().Select(mapper.Map<NoteResponseDto>).ToList();
    }

    public NoteResponseDto? Update(long id, NoteRequestDto dto)
    {
        var note = mapper.Map<Note>(dto);

        var validationResult = validator.Validate(note);

        if (validationResult.IsValid)
        {
            if (note.IssueId == -1 || issueRepository.Exist(note.IssueId))
            {
                var updatedNote = noteRepository.Update(id, note);

                if (updatedNote is not null)
                {
                    return mapper.Map<NoteResponseDto>(updatedNote);
                }
            }
        }

        return null;
    }

    public void Delete(long id)
    {
        noteRepository.Delete(id);
    }
}