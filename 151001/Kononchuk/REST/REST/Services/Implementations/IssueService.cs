using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;

namespace REST.Services.Implementations;

public class IssueService(
    IMapper mapper,
    IIssueRepository<long> issueRepository,
    IEditorRepository<long> editorRepository,
    AbstractValidator<Issue> validator)
    : IIssueService
{
    public IssueResponseDto? Create(IssueRequestDto dto)
    {
        var issue = mapper.Map<Issue>(dto);

        var validationResult = validator.Validate(issue);

        if (validationResult.IsValid)
        {
            if (issue.EditorId == -1 || editorRepository.Exist(issue.EditorId))
            {
                var createdIssue = issueRepository.Add(issue);

                if (createdIssue is not null)
                {
                    createdIssue.Created = DateTime.Now;
                    createdIssue.Modified = issue.Created;

                    return mapper.Map<IssueResponseDto>(createdIssue);
                }   
            }
        }

        return null;
    }

    public IssueResponseDto? GetById(long id)
    {
        var foundIssue = issueRepository.GetById(id);

        if (foundIssue is not null)
        {
            return mapper.Map<IssueResponseDto>(foundIssue);
        }

        return null;
    }

    public List<IssueResponseDto> GetAll()
    {
        return issueRepository.GetAll().Select(mapper.Map<IssueResponseDto>).ToList();
    }

    public IssueResponseDto? Update(long id, IssueRequestDto dto)
    {
        var issue = mapper.Map<Issue>(dto);

        var validationResult = validator.Validate(issue);

        if (validationResult.IsValid)
        {
            if (issue.EditorId == -1 || editorRepository.Exist(issue.EditorId))
            {
                var updatedIssue = issueRepository.Update(id, issue);

                if (updatedIssue is not null)
                {
                    updatedIssue.Modified = DateTime.Now;

                    return mapper.Map<IssueResponseDto>(updatedIssue);
                }
            }
        }

        return null;
    }

    public void Delete(long id)
    {
        issueRepository.Delete(id);
    }
}