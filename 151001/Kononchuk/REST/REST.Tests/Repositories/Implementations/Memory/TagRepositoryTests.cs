using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

[TestSubject(typeof(TagRepository))]
public class TagRepositoryTests
{
    private async Task<TagRepository> PrepareRepositoryAsync()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag{ Name = "created"};

        await repository.AddAsync(tag);

        return repository;
    }

    [Fact]
    public async Task AddAsync_NullArgument_ThrowArgumentNullException()
    {
        TagRepository repository = new TagRepository();

        async Task Actual() => await repository.AddAsync(null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task AddAsync_ValidTag_ReturnTagWithSetId()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag { Name = "created"};

        var addedTag = await repository.AddAsync(tag);
        
        Assert.Equal(1, addedTag.Id);
        Assert.Equal(tag.Name, addedTag.Name);
    }

    [Fact]
    public async Task UpdateAsync_NullArgument_ThrowArgumentNullException()
    {
        TagRepository repository = new TagRepository();

        async Task Actual() => await repository.UpdateAsync(1, null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task UpdateAsync_TagNotExist_ThrowResourceNotFoundException()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag { Name = "updated" };

        async Task<Tag> Actual() => await repository.UpdateAsync(-1, tag);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }

    [Fact]
    public async Task UpdateAsync_ValidArguments_ReturnUpdatedTag()
    {
        TagRepository repository = await PrepareRepositoryAsync();
        Tag tag = new Tag { Name = "created" };

        var updateTag = await repository.UpdateAsync(1, tag);

        Assert.Equal(tag.Name, updateTag.Name);
    }
}