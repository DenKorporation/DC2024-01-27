using JetBrains.Annotations;
using REST.Publisher.Models.Entities;
using REST.Publisher.Repositories.Implementations.Memory;
using REST.Publisher.Utilities.Exceptions;

namespace REST.Publisher.UnitTests.Repositories.Implementations.Memory;

[TestSubject(typeof(IssueRepository))]
public class IssueRepositoryTests
{
    private async Task<IssueRepository> PrepareRepositoryAsync()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue { Title = "created"};

        await repository.AddAsync(issue);

        return repository;
    }

    [Fact]
    public async Task AddAsync_NullArgument_ThrowsArgumentNullException()
    {
        IssueRepository repository = new IssueRepository();

        async Task Actual() => await repository.AddAsync(null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task AddAsync_ValidIssue_ReturnsIssueWithSetId()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue { Title = "created"};

        var addedIssue = await repository.AddAsync(issue);

        Assert.Equal(1, addedIssue.Id);
        Assert.Equal(issue.Title, addedIssue.Title);
    }

    [Fact]
    public async Task UpdateAsync_NullArgument_ThrowsArgumentNullException()
    {
        IssueRepository repository = new IssueRepository();

        async Task Actual() => await repository.UpdateAsync(1, null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task UpdateAsync_IssueNotExist_ThrowsResourceNotFoundException()
    {
        IssueRepository repository = new IssueRepository();
        Issue issue = new Issue { Title = "updated" };

        async Task<Issue> Actual() => await repository.UpdateAsync(-1, issue);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }

    [Fact]
    public async Task UpdateAsync_ValidArguments_ReturnsUpdatedIssue()
    {
        IssueRepository repository = await PrepareRepositoryAsync();
        Issue issue = new Issue { Title = "updated" };

        var updateIssue = await repository.UpdateAsync(1, issue);

        Assert.Equal(issue.Title, updateIssue.Title);
    }
}