using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;
using ValidationException = REST.Utilities.Exceptions.ValidationException;

namespace REST.Services.Implementations;

public class EditorService(
    IMapper mapper,
    IEditorRepository<long> editorRepository,
    AbstractValidator<Editor> validator) : IEditorService
{
    public async Task<EditorResponseDto> CreateAsync(EditorRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var editor = mapper.Map<Editor>(dto);

        var validationResult = await validator.ValidateAsync(editor);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Editor data has not been validated", 40001);
        }

        var createdEditor = await editorRepository.AddAsync(editor);

        return mapper.Map<EditorResponseDto>(createdEditor);
    }

    public async Task<EditorResponseDto> GetByIdAsync(long id)
    {
        var foundEditor = await editorRepository.GetByIdAsync(id);

        return mapper.Map<EditorResponseDto>(foundEditor);
    }

    public async Task<IEnumerable<EditorResponseDto>> GetAllAsync()
    {
        return (await editorRepository.GetAllAsync()).Select(mapper.Map<EditorResponseDto>).ToList();
    }

    public async Task<EditorResponseDto> UpdateAsync(long id, EditorRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var editor = mapper.Map<Editor>(dto);

        var validationResult = await validator.ValidateAsync(editor);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Editor data has not been validated", 40002);
        }

        var updatedEditor = await editorRepository.UpdateAsync(id, editor);

        return mapper.Map<EditorResponseDto>(updatedEditor);
    }

    public async Task DeleteAsync(long id)
    {
        await editorRepository.DeleteAsync(id);
    }
}