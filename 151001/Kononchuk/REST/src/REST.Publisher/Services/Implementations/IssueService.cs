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
            throw new Exceptions_ValidationException("Issue data has not been validated", 40001);
        }

        var createdIssue = await issueRepository.AddAsync(issue);

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
            throw new Exceptions_ValidationException("Issue data has not been validated", 40002);
        }

        var updatedIssue = await issueRepository.UpdateAsync(id, issue);

        return mapper.Map<IssueResponseDto>(updatedIssue);
    }

    public async Task DeleteAsync(long id)
    {
        await issueRepository.DeleteAsync(id);
    }
}