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

[TestSubject(typeof(TagService))]
public class TagServiceTest
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ITagRepository<long>> _repositoryMock = new();
    private readonly Mock<AbstractValidator<Tag>> _validatorMock = new();
    private readonly ITagService _tagService;

    public TagServiceTest()
    {
        _tagService = new TagService(_mapperMock.Object, _repositoryMock.Object, _validatorMock.Object);
    }


    [Fact]
    public async Task CreateAsync_NullArgument_ThrowsArgumentNullException()
    {
        Func<Task> actual = async () => await _tagService.CreateAsync(null!);

        await actual.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAsync_InvalidArgument_ThrowsValidationException()
    {
        TagRequestDto tagRequestDto = new();
        Tag tag = new();
        ValidationResult validationResult =
            new ValidationResult(new List<ValidationFailure>([new ValidationFailure()]));
        _mapperMock.Setup(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>())).Returns(tag);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        Func<Task> actual = async () => await _tagService.CreateAsync(tagRequestDto);

        await actual.Should().ThrowExactlyAsync<ValidationException>();
        _mapperMock.Verify(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ValidArgument_ReturnsAddedTagResponseDto()
    {
        TagRequestDto tagRequestDto = new();
        TagResponseDto tagResponseDto = new();
        Tag tag = new();
        ValidationResult validationResult = new();
        _mapperMock.Setup(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>())).Returns(tag);
        _mapperMock.Setup(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>())).Returns(tagResponseDto);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _repositoryMock.Setup(repository => repository.AddAsync(It.IsAny<Tag>())).ReturnsAsync(tag);

        var result = await _tagService.CreateAsync(tagRequestDto);

        result.Should().Be(tagResponseDto);
        _mapperMock.Verify(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Tag>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_TagNotExist_ThrowsResourceNotFoundException()
    {
        _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(new ResourceNotFoundException());

        Func<Task> actual = async () => await _tagService.GetByIdAsync(-1);

        await actual.Should().ThrowExactlyAsync<ResourceNotFoundException>();
        _repositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_TagExist_ReturnsExistingResult()
    {
        Tag tag = new Tag();
        TagResponseDto tagResponseDto = new();
        _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(tag);
        _mapperMock.Setup(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>())).Returns(tagResponseDto);

        var result = await _tagService.GetByIdAsync(1);

        result.Should().Be(tagResponseDto);
        _repositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
    {
        TagResponseDto tagResponseDto = new();
        _repositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([]);
        _mapperMock.Setup(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>())).Returns(tagResponseDto);

        var result = await _tagService.GetAllAsync();

        result.Should().BeEmpty();
        _repositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_NullArgument_ThrowsArgumentNullException()
    {
        Func<Task> actual = async () => await _tagService.UpdateAsync(1, null!);

        await actual.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_InvalidArgument_ThrowsValidationException()
    {
        TagRequestDto tagRequestDto = new();
        Tag tag = new();
        ValidationResult validationResult =
            new ValidationResult(new List<ValidationFailure>([new ValidationFailure()]));
        _mapperMock.Setup(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>())).Returns(tag);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        Func<Task> actual = async () => await _tagService.UpdateAsync(1, tagRequestDto);

        await actual.Should().ThrowExactlyAsync<ValidationException>();
        _mapperMock.Verify(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ValidArgument_ReturnsAddedTagResponseDto()
    {
        TagRequestDto tagRequestDto = new();
        TagResponseDto tagResponseDto = new();
        Tag tag = new();
        ValidationResult validationResult = new();
        _mapperMock.Setup(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>())).Returns(tag);
        _mapperMock.Setup(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>())).Returns(tagResponseDto);
        _validatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _repositoryMock.Setup(repository => repository.UpdateAsync(It.IsAny<long>(), It.IsAny<Tag>()))
            .ReturnsAsync(tag);

        var result = await _tagService.UpdateAsync(1, tagRequestDto);

        result.Should().Be(tagResponseDto);
        _mapperMock.Verify(mapper => mapper.Map<Tag>(It.IsAny<TagRequestDto>()), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<TagResponseDto>(It.IsAny<Tag>()), Times.Once);
        _validatorMock.Verify(
            validator => validator.ValidateAsync(It.IsAny<ValidationContext<Tag>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(repository => repository.UpdateAsync(It.IsAny<long>(), It.IsAny<Tag>()), Times.Once);
    }

    [Fact]
    public async Task Delete_TagNotExist_ThrowsResourceNotFoundException()
    {
        _repositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<long>()))
            .ThrowsAsync(new ResourceNotFoundException());

        Func<Task> actual = async () => await _tagService.DeleteAsync(-1);

        await actual.Should().ThrowExactlyAsync<ResourceNotFoundException>();
        _repositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task Delete_TagExist_TagNoLongerExist()
    {
        _repositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<long>()));

        Func<Task> actual = async () => await _tagService.DeleteAsync(1);

        await actual.Should().NotThrowAsync();
        _repositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<long>()), Times.Once);
    }
}