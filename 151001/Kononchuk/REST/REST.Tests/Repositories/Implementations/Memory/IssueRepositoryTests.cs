using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

[TestSubject(typeof(IssueRepository))]
public class IssueRepositoryTests
{
    private IssueRepository PrepareRepository()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue { Title = "created"};

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
        Issue issue = new Issue { Title = "created"};

        var addedIssue = repository.Add(issue);

        Assert.Equal(1, addedIssue.Id);
        Assert.Equal(issue.Title, addedIssue.Title);
    }

    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        IssueRepository repository = new IssueRepository();

        void Actual() => repository.Update(1, null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_IssueNotExist_ThrowResourceNotFoundException()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue { Title = "updated" };

        Issue Actual() => repository.Update(-1, issue);

        Assert.Throws<ResourceNotFoundException>(Actual);
    }

    [Fact]
    public void Update_ValidArguments_ReturnUpdatedIssue()
    {
        IssueRepository repository = PrepareRepository();
        Issue issue = new Issue { Title = "updated" };

        var updateIssue = repository.Update(1, issue);

        Assert.Equal(issue.Title, updateIssue.Title);
    }
}