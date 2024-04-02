using AutoMapper;
using FluentValidation;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Models.Entities;
using REST.Publisher.Repositories.Interfaces;
using REST.Publisher.Services.Interfaces;
using Exceptions_ValidationException = REST.Publisher.Utilities.Exceptions.ValidationException;
using ValidationException = REST.Publisher.Utilities.Exceptions.ValidationException;

namespace REST.Publisher.Services.Implementations;

public class TagService(
    IMapper mapper,
    ITagRepository<long> tagRepository,
    AbstractValidator<Tag> validator) : ITagService
{
    public async Task<TagResponseDto> CreateAsync(TagRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var tag = mapper.Map<Tag>(dto);

        var validationResult = await validator.ValidateAsync(tag);

        if (!validationResult.IsValid)
        {
            throw new Exceptions_ValidationException("Tag data has not been validated", 40001);
        }

        var createdTag = await tagRepository.AddAsync(tag);

        return mapper.Map<TagResponseDto>(createdTag);
    }

    public async Task<TagResponseDto> GetByIdAsync(long id)
    {
        var foundTag = await tagRepository.GetByIdAsync(id);

        var responseDto = mapper.Map<TagResponseDto>(foundTag);
        return responseDto;
    }

    public async Task<IEnumerable<TagResponseDto>> GetAllAsync()
    {
        return (await tagRepository.GetAllAsync()).Select(mapper.Map<TagResponseDto>).ToList();
    }

    public async Task<TagResponseDto> UpdateAsync(long id, TagRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var tag = mapper.Map<Tag>(dto);

        var validationResult = await validator.ValidateAsync(tag);

        if (!validationResult.IsValid)
        {
            throw new Exceptions_ValidationException("Tag data has not been validated", 40002);
        }

        var updatedTag = await tagRepository.UpdateAsync(id, tag);

        return mapper.Map<TagResponseDto>(updatedTag);
    }

    public async Task DeleteAsync(long id)
    {
        await tagRepository.DeleteAsync(id);
    }
}