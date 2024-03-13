using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

[TestSubject(typeof(TagRepository))]
public class TagRepositoryTests
{
    private TagRepository PrepareRepository()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag{ Name = "created"};

        repository.Add(tag);

        return repository;
    }

    [Fact]
    public void Add_NullArgument_ThrowArgumentNullException()
    {
        TagRepository repository = new TagRepository();

        void Actual() => repository.Add(null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Add_ValidTag_ReturnTagWithSetId()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag { Name = "created"};

        var addedTag = repository.Add(tag);
        
        Assert.Equal(1, addedTag.Id);
        Assert.Equal(tag.Name, addedTag.Name);
    }

    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        TagRepository repository = new TagRepository();

        void Actual() => repository.Update(1, null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_TagNotExist_ThrowResourceNotFoundException()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag { Name = "updated" };

        Tag Actual() => repository.Update(-1, tag);

        Assert.Throws<ResourceNotFoundException>(Actual);
    }

    [Fact]
    public void Update_ValidArguments_ReturnUpdatedTag()
    {
        TagRepository repository = PrepareRepository();
        Tag tag = new Tag { Name = "created" };

        var updateTag = repository.Update(1, tag);

        Assert.Equal(tag.Name, updateTag.Name);
    }
}