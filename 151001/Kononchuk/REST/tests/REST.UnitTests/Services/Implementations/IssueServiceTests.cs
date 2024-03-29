using AutoMapper;
using FluentValidation;
using FluentAssertions;
using FluentValidation.Results;
using JetBrains.Annotations;
using Moq;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Services.Implementations;
using REST.Services.Interfaces;
using REST.Utilities.Exceptions;
using ValidationException = REST.Utilities.Exceptions.ValidationException;


namespace REST.UnitTests.Services.Implementations;

[TestSubject(typeof(IssueService))]
public class IssueServiceTest
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IIssueRepository<long>> _repositoryMock = new();
    private readonly Mock<AbstractValidator<Issue>> _validatorMock = new();
    private readonly IIssueService _issueService;

    public IssueServiceTest()
    {
        _issueService = new IssueService(_mapperMock.Object, _repositoryMock.Object, _validatorMock.Object);
    }


    [Fact]
    public async Task CreateAsync_NullArgument_ThrowsArgumentNullException()
    {
        Func<Task> actual = async () => await _issueService.CreateAsync(null!);

        await actual.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAsync_InvalidArgument_ThrowsValidationException()
    {
        IssueRequestDto issueRequestDto = new();
        Issue issue = new();
        ValidationResult validationResult =
            new ValidationResult(new List<ValidationFailure>([new ValidationFailure()]));
        _mapperMock.Setup(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>())).Returns(issue);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        Func<Task> actual = async () => await _issueService.CreateAsync(issueRequestDto);

        await actual.Should().ThrowExactlyAsync<ValidationException>();
        _mapperMock.Verify(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ValidArgument_ReturnsAddedIssueResponseDto()
    {
        IssueRequestDto issueRequestDto = new();
        IssueResponseDto issueResponseDto = new();
        Issue issue = new();
        ValidationResult validationResult = new();
        _mapperMock.Setup(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>())).Returns(issue);
        _mapperMock.Setup(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>())).Returns(issueResponseDto);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _repositoryMock.Setup(repository => repository.AddAsync(It.IsAny<Issue>())).ReturnsAsync(issue);

        var result = await _issueService.CreateAsync(issueRequestDto);

        result.Should().Be(issueResponseDto);
        _mapperMock.Verify(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Issue>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_IssueNotExist_ThrowsResourceNotFoundException()
    {
        _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(new ResourceNotFoundException());

        Func<Task> actual = async () => await _issueService.GetByIdAsync(-1);

        await actual.Should().ThrowExactlyAsync<ResourceNotFoundException>();
        _repositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_IssueExist_ReturnsExistingResult()
    {
        Issue issue = new Issue();
        IssueResponseDto issueResponseDto = new();
        _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(issue);
        _mapperMock.Setup(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>())).Returns(issueResponseDto);

        var result = await _issueService.GetByIdAsync(1);

        result.Should().Be(issueResponseDto);
        _repositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
    {
        IssueResponseDto issueResponseDto = new();
        _repositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([]);
        _mapperMock.Setup(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>())).Returns(issueResponseDto);

        var result = await _issueService.GetAllAsync();

        result.Should().BeEmpty();
        _repositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_NullArgument_ThrowsArgumentNullException()
    {
        Func<Task> actual = async () => await _issueService.UpdateAsync(1, null!);

        await actual.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_InvalidArgument_ThrowsValidationException()
    {
        IssueRequestDto issueRequestDto = new();
        Issue issue = new();
        ValidationResult validationResult =
            new ValidationResult(new List<ValidationFailure>([new ValidationFailure()]));
        _mapperMock.Setup(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>())).Returns(issue);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        Func<Task> actual = async () => await _issueService.UpdateAsync(1, issueRequestDto);

        await actual.Should().ThrowExactlyAsync<ValidationException>();
        _mapperMock.Verify(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ValidArgument_ReturnsAddedIssueResponseDto()
    {
        IssueRequestDto issueRequestDto = new();
        IssueResponseDto issueResponseDto = new();
        Issue issue = new();
        ValidationResult validationResult = new();
        _mapperMock.Setup(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>())).Returns(issue);
        _mapperMock.Setup(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>())).Returns(issueResponseDto);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _repositoryMock.Setup(repository => repository.UpdateAsync(It.IsAny<long>(), It.IsAny<Issue>()))
            .ReturnsAsync(issue);

        var result = await _issueService.UpdateAsync(1, issueRequestDto);

        result.Should().Be(issueResponseDto);
        _mapperMock.Verify(mapper => mapper.Map<Issue>(It.IsAny<IssueRequestDto>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IssueResponseDto>(It.IsAny<Issue>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Issue>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(repository => repository.UpdateAsync(It.IsAny<long>(), It.IsAny<Issue>()), Times.Once);
    }

    [Fact]
    public async Task Delete_IssueNotExist_ThrowsResourceNotFoundException()
    {
        _repositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<long>()))
            .ThrowsAsync(new ResourceNotFoundException());

        Func<Task> actual = async () => await _issueService.DeleteAsync(-1);

        await actual.Should().ThrowExactlyAsync<ResourceNotFoundException>();
        _repositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task Delete_IssueExist_IssueNoLongerExist()
    {
        _repositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<long>()));

        Func<Task> actual = async () => await _issueService.DeleteAsync(1);

        await actual.Should().NotThrowAsync();
        _repositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<long>()), Times.Once);
    }
}