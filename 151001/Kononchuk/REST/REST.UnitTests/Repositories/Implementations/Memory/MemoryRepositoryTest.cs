using REST.UnitTests.Repositories.Implementations.Memory.Mocks;
using REST.Utilities.Exceptions;

namespace REST.UnitTests.Repositories.Implementations.Memory;

public class MemoryRepositoryTest
{
    private async Task<TestableRepository> PrepareRepositoryAsync()
    {
        TestableRepository repository = new TestableRepository();
        string element = "test";

        await repository.AddAsync(element);

        return repository;
    }

    [Fact]
    public async Task ExistAsync_NotExist_ReturnsFalse()
    {
        TestableRepository repository = new TestableRepository();

        bool isExist = await repository.ExistAsync(-1);

        Assert.False(isExist);
    }

    [Fact]
    public async Task ExistAsync_Exist_ReturnsTrue()
    {
        TestableRepository repository = await PrepareRepositoryAsync();

        bool isExist = await repository.ExistAsync(1);

        Assert.True(isExist);
    }
    
    [Fact]
    public async Task GetByIdAsync_NotExist_ThrowsResourceNotFoundException()
    {
        TestableRepository repository = await PrepareRepositoryAsync();

        async Task<string> Actual() => await repository.GetByIdAsync(-1);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }
    
    [Fact]
    public async Task GetByIdAsync_Exist_ReturnsExistingResult()
    {
        TestableRepository repository = await PrepareRepositoryAsync();

        var element = await repository.GetByIdAsync(1);

        Assert.NotNull(element);
    }
    
    [Fact]
    public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
    {
        TestableRepository repository = new TestableRepository();

        var allElements = await repository.GetAllAsync();

        Assert.Empty(allElements);
    }
    
    [Fact]
    public async Task GetAllAsync_NonEmptyRepository_ReturnsNonEmptyList()
    {
        TestableRepository repository = await PrepareRepositoryAsync();

        var allElements = await repository.GetAllAsync();

        Assert.NotEmpty(allElements);
    }
    
    [Fact]
    public async Task DeleteAsync_ElementExist_RepositoryNoLongerContainsThisElement()
    {
        TestableRepository repository = await PrepareRepositoryAsync();

        await repository.DeleteAsync(1);
        
        Assert.False(await repository.ExistAsync(1));
    }
    
    [Fact]
    public async Task DeleteAsync_ElementNotExist_ThrowsResourceNotFoundException()
    {
        TestableRepository repository = await PrepareRepositoryAsync();

        async Task Actual() => await repository.DeleteAsync(-1);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }
}