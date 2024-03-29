using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;
using ValidationException = REST.Utilities.Exceptions.ValidationException;

namespace REST.Services.Implementations;

public class NoteService(
    IMapper mapper,
    INoteRepository<long> noteRepository,
    AbstractValidator<Note> validator) : INoteService
{
    public async Task<NoteResponseDto> CreateAsync(NoteRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var note = mapper.Map<Note>(dto);

        var validationResult = await validator.ValidateAsync(note);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Note data has not been validated", 40001);
        }

        var createdNote = await noteRepository.AddAsync(note);

        return mapper.Map<NoteResponseDto>(createdNote);
    }


    public async Task<NoteResponseDto> GetByIdAsync(long id)
    {
        var foundNote = await noteRepository.GetByIdAsync(id);

        return mapper.Map<NoteResponseDto>(foundNote);
    }

    public async Task<IEnumerable<NoteResponseDto>> GetAllAsync()
    {
        return (await noteRepository.GetAllAsync()).Select(mapper.Map<NoteResponseDto>).ToList();
    }

    public async Task<NoteResponseDto> UpdateAsync(long id, NoteRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var note = mapper.Map<Note>(dto);

        var validationResult = await validator.ValidateAsync(note);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Note data has not been validated", 40002);
        }

        var updatedNote = await noteRepository.UpdateAsync(id, note);

        return mapper.Map<NoteResponseDto>(updatedNote);
    }

    public async Task DeleteAsync(long id)
    {
        await noteRepository.DeleteAsync(id);
    }
}