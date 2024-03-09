using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;

namespace REST.Tests.Repositories.Memory;

public class IssueRepositoryTests
{
    private IssueRepository PrepareRepository()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue() { Title = "created"};

        repository.Add(issue);

        return repository;
    }

    [Fact]
    public void Add_NullArgument_ThrowArgumentNullException()
    {
        IssueRepository repository = new IssueRepository();

        void Actual() => repository.Add(null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Add_ValidIssue_ReturnIssueWithSetId()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue() { Title = "created"};

        var addedIssue = repository.Add(issue);


        Assert.True(addedIssue?.Id == 1);
    }

    [Fact]
    public void Exist_NotExist_ReturnFalse()
    {
        IssueRepository repository = new IssueRepository();

        bool isExist = repository.Exist(-1);

        Assert.False(isExist);
    }

    [Fact]
    public void Exist_Exist_ReturnTrue()
    {
        IssueRepository repository = PrepareRepository();

        bool isExist = repository.Exist(1);

        Assert.True(isExist);
    }

    [Fact]
    public void GetById_NotExist_ReturnNull()
    {
        IssueRepository repository = PrepareRepository();

        var issue = repository.GetById(-1);

        Assert.Null(issue);
    }

    [Fact]
    public void GetById_Exist_ReturnExistingResult()
    {
        IssueRepository repository = PrepareRepository();

        var issue = repository.GetById(1);

        Assert.NotNull(issue);
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnEmptyList()
    {
        IssueRepository repository = new IssueRepository();

        var allIssues = repository.GetAll();

        Assert.Empty(allIssues);
    }

    [Fact]
    public void GetAll_NonEmptyRepository_ReturnNonEmptyList()
    {
        IssueRepository repository = PrepareRepository();

        var allIssues = repository.GetAll();

        Assert.NotEmpty(allIssues);
    }

    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        IssueRepository repository = new IssueRepository();

        void Actual() => repository.Update(1, null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_IssueNotExist_ReturnNull()
    {
        IssueRepository repository = PrepareRepository();
        Issue issue = new Issue() { Title = "updated" };

        var updateIssue = repository.Update(-1, issue);

        Assert.Null(updateIssue);
    }

    [Fact]
    public void Update_ValidArguments_ReturnUpdatedIssue()
    {
        IssueRepository repository = PrepareRepository();
        Issue issue = new Issue() { Title = "created" };

        var updateIssue = repository.Update(1, issue);

        Assert.Equal(issue.Title, updateIssue?.Title);
    }

    [Fact]
    public void Delete_IssueExist_ReturnTrue()
    {
        IssueRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(1);

        Assert.True(isDeleted);
    }

    [Fact]
    public void Delete_IssueNotExist_ReturnFalse()
    {
        IssueRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(-1);

        Assert.False(isDeleted);
    }
}