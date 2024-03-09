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
    IIssueTagRepository<long> issueTagRepository,
    AbstractValidator<Tag> validator) : ITagService
{
    public TagResponseDto? Create(TagRequestDto dto)
    {
        var tag = mapper.Map<Tag>(dto);

        var validationResult = validator.Validate(tag);

        if (validationResult.IsValid)
        {
            var createdTag = tagRepository.Add(tag);

            if (createdTag is not null)
            {
                if (dto.IssueId == -1 ||
                    issueTagRepository.Add(new IssueTag() { IssueId = dto.IssueId, TagId = createdTag.Id }) is not null)
                {
                    var responseDto = mapper.Map<TagResponseDto>(createdTag);
                    responseDto.IssueId = dto.IssueId;
                    return responseDto;
                }
            }
        }

        return null;
    }

    public TagResponseDto? GetById(long id)
    {
        var foundTag = tagRepository.GetById(id);

        if (foundTag is not null)
        {
            var responseDto = mapper.Map<TagResponseDto>(foundTag);
            var issueTags = issueTagRepository.GetByTagId(responseDto.Id);
            responseDto.IssueId = issueTags.FirstOrDefault()?.IssueId ?? -1;
            return responseDto;
        }

        return null;
    }

    public List<TagResponseDto> GetAll()
    {
        return tagRepository.GetAll().Select(tag =>
        {
            var responseDto = mapper.Map<TagResponseDto>(tag);
            var issueTags = issueTagRepository.GetByTagId(responseDto.Id);
            responseDto.IssueId = issueTags.FirstOrDefault()?.IssueId ?? -1;
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

            if (updatedTag is not null)
            {
                if (dto.IssueId == -1 ||
                    issueTagRepository.Add(new IssueTag()
                        { IssueId = dto.IssueId, TagId = updatedTag.Id }) is not null)
                {
                    var responseDto = mapper.Map<TagResponseDto>(updatedTag);
                    responseDto.IssueId = dto.IssueId;
                    return responseDto;
                }
            }
        }

        return null;
    }

    public bool Delete(long id)
    {
        return tagRepository.Delete(id);
    }
}