using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;

namespace REST.Services.Implementations;

public class TagService(
    IMapper mapper,
    ITagRepository<long> tagRepository,
    AbstractValidator<Tag> validator) : ITagService
{
    public TagResponseDto? Create(TagRequestDto dto)
    {
        var tag = mapper.Map<Tag>(dto);

        var validationResult = validator.Validate(tag);

        if (validationResult.IsValid)
        {
            var createdTag = tagRepository.Add(tag);
            
            return mapper.Map<TagResponseDto>(createdTag);
        }

        return null;
    }

    public TagResponseDto? GetById(long id)
    {
        var foundTag = tagRepository.GetById(id);

        if (foundTag is not null)
        {
            var responseDto = mapper.Map<TagResponseDto>(foundTag);
            return responseDto;
        }

        return null;
    }

    public List<TagResponseDto> GetAll()
    {
        return tagRepository.GetAll().Select(tag =>
        {
            var responseDto = mapper.Map<TagResponseDto>(tag);
            return responseDto;
        }).ToList();
    }

    public TagResponseDto? Update(long id, TagRequestDto dto)
    {
        var tag = mapper.Map<Tag>(dto);

        var validationResult = validator.Validate(tag);

        if (validationResult.IsValid)
        {
            var updatedTag = tagRepository.Update(id, tag);

            return mapper.Map<TagResponseDto>(updatedTag);
        }

        return null;
    }

    public void Delete(long id)
    {
        tagRepository.Delete(id);
    }
}