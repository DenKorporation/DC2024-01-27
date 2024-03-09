using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;

namespace REST.Tests.Repositories.Memory;

public class TagRepositoryTests
{
    private TagRepository PrepareRepository()
    {
        TagRepository repository = new TagRepository();
        Tag tag = new Tag() { Name = "created"};

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
        Tag tag = new Tag() { Name = "created"};

        var addedTag = repository.Add(tag);


        Assert.True(addedTag?.Id == 1);
    }

    [Fact]
    public void Exist_NotExist_ReturnFalse()
    {
        TagRepository repository = new TagRepository();

        bool isExist = repository.Exist(-1);

        Assert.False(isExist);
    }

    [Fact]
    public void Exist_Exist_ReturnTrue()
    {
        TagRepository repository = PrepareRepository();

        bool isExist = repository.Exist(1);

        Assert.True(isExist);
    }

    [Fact]
    public void GetById_NotExist_ReturnNull()
    {
        TagRepository repository = PrepareRepository();

        var tag = repository.GetById(-1);

        Assert.Null(tag);
    }

    [Fact]
    public void GetById_Exist_ReturnExistingResult()
    {
        TagRepository repository = PrepareRepository();

        var tag = repository.GetById(1);

        Assert.NotNull(tag);
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnEmptyList()
    {
        TagRepository repository = new TagRepository();

        var allTags = repository.GetAll();

        Assert.Empty(allTags);
    }

    [Fact]
    public void GetAll_NonEmptyRepository_ReturnNonEmptyList()
    {
        TagRepository repository = PrepareRepository();

        var allTags = repository.GetAll();

        Assert.NotEmpty(allTags);
    }

    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        TagRepository repository = new TagRepository();

        void Actual() => repository.Update(1, null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_TagNotExist_ReturnNull()
    {
        TagRepository repository = PrepareRepository();
        Tag tag = new Tag() { Name = "updated" };

        var updateTag = repository.Update(-1, tag);

        Assert.Null(updateTag);
    }

    [Fact]
    public void Update_ValidArguments_ReturnUpdatedTag()
    {
        TagRepository repository = PrepareRepository();
        Tag tag = new Tag() { Name = "created" };

        var updateTag = repository.Update(1, tag);

        Assert.Equal(tag.Name, updateTag?.Name);
    }

    [Fact]
    public void Delete_TagExist_ReturnTrue()
    {
        TagRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(1);

        Assert.True(isDeleted);
    }

    [Fact]
    public void Delete_TagNotExist_ReturnFalse()
    {
        TagRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(-1);

        Assert.False(isDeleted);
    }
}