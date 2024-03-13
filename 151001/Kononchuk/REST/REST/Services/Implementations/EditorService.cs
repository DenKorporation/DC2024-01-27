using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;

namespace REST.Services.Implementations;

public class EditorService(
    IMapper mapper,
    IEditorRepository<long> editorRepository,
    AbstractValidator<Editor> validator) : IEditorService
{
    public EditorResponseDto? Create(EditorRequestDto dto)
    {
        var editor = mapper.Map<Editor>(dto);

        var validationResult = validator.Validate(editor);

        if (validationResult.IsValid)
        {
            var createdEditor = editorRepository.Add(editor);

            if (createdEditor is not null)
            {
                return mapper.Map<EditorResponseDto>(createdEditor);
            }
        }

        return null;
    }

    public EditorResponseDto? GetById(long id)
    {
        var foundEditor = editorRepository.GetById(id);

        if (foundEditor is not null)
        {
            return mapper.Map<EditorResponseDto>(foundEditor);
        }

        return null;
    }

    public List<EditorResponseDto> GetAll()
    {
        return editorRepository.GetAll().Select(mapper.Map<EditorResponseDto>).ToList();
    }

    public EditorResponseDto? Update(long id, EditorRequestDto dto)
    {
        var editor = mapper.Map<Editor>(dto);

        var validationResult = validator.Validate(editor);

        if (validationResult.IsValid)
        {
            var updatedEditor = editorRepository.Update(id, editor);

            if (updatedEditor is not null)
            {
                return mapper.Map<EditorResponseDto>(updatedEditor);
            }
        }

        return null;
    }

    public void Delete(long id)
    {
        editorRepository.Delete(id);
    }
}