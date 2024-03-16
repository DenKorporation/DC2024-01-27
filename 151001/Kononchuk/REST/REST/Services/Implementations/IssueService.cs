using AutoMapper;
using FluentValidation;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Interfaces;
using ValidationException = REST.Utilities.Exceptions.ValidationException;

namespace REST.Services.Implementations;

public class IssueService(
    IMapper mapper,
    IIssueRepository<long> issueRepository,
    AbstractValidator<Issue> validator)
    : IIssueService
{
    public async Task<IssueResponseDto> CreateAsync(IssueRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var issue = mapper.Map<Issue>(dto);

        var validationResult = await validator.ValidateAsync(issue);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Issue data has not been validated", 40001);
        }

        var createdIssue = await issueRepository.AddAsync(issue);

        createdIssue.Created = DateTime.Now;
        createdIssue.Modified = issue.Created;

        return mapper.Map<IssueResponseDto>(createdIssue);
    }

    public async Task<IssueResponseDto> GetByIdAsync(long id)
    {
        var foundIssue = await issueRepository.GetByIdAsync(id);

        return mapper.Map<IssueResponseDto>(foundIssue);
    }

    public async Task<IEnumerable<IssueResponseDto>> GetAllAsync()
    {
        return (await issueRepository.GetAllAsync()).Select(mapper.Map<IssueResponseDto>).ToList();
    }

    public async Task<IssueResponseDto> UpdateAsync(long id, IssueRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var issue = mapper.Map<Issue>(dto);

        var validationResult = await validator.ValidateAsync(issue);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Issue data has not been validated", 40002);
        }

        var updatedIssue = await issueRepository.UpdateAsync(id, issue);

        updatedIssue.Modified = DateTime.Now;

        return mapper.Map<IssueResponseDto>(updatedIssue);
    }

    public async Task DeleteAsync(long id)
    {
        await issueRepository.DeleteAsync(id);
    }
}