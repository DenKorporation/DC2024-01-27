using REST.Tests.Repositories.Implementations.Memory.Utiles;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

public class MemoryRepositoryTest
{
    private async Task<MemoryRepositoryImplementation> PrepareRepositoryAsync()
    {
        MemoryRepositoryImplementation repository = new MemoryRepositoryImplementation();
        string element = "test";

        await repository.AddAsync(element);

        return repository;
    }

    [Fact]
    public async Task ExistAsync_NotExist_ReturnFalse()
    {
        MemoryRepositoryImplementation repository = new MemoryRepositoryImplementation();

        bool isExist = await repository.ExistAsync(-1);

        Assert.False(isExist);
    }

    [Fact]
    public async Task ExistAsync_Exist_ReturnTrue()
    {
        MemoryRepositoryImplementation repository = await PrepareRepositoryAsync();

        bool isExist = await repository.ExistAsync(1);

        Assert.True(isExist);
    }
    
    [Fact]
    public async Task GetByIdAsync_NotExist_ThrowResourceNotFoundException()
    {
        MemoryRepositoryImplementation repository = await PrepareRepositoryAsync();

        async Task<string> Actual() => await repository.GetByIdAsync(-1);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }
    
    [Fact]
    public async Task GetByIdAsync_Exist_ReturnExistingResult()
    {
        MemoryRepositoryImplementation repository = await PrepareRepositoryAsync();

        var element = await repository.GetByIdAsync(1);

        Assert.NotNull(element);
    }
    
    [Fact]
    public async Task GetAllAsync_EmptyRepository_ReturnEmptyList()
    {
        MemoryRepositoryImplementation repository = new MemoryRepositoryImplementation();

        var allElements = await repository.GetAllAsync();

        Assert.Empty(allElements);
    }
    
    [Fact]
    public async Task GetAllAsync_NonEmptyRepository_ReturnNonEmptyList()
    {
        MemoryRepositoryImplementation repository = await PrepareRepositoryAsync();

        var allElements = await repository.GetAllAsync();

        Assert.NotEmpty(allElements);
    }
    
    [Fact]
    public async Task DeleteAsync_ElementExist_RepositoryNoLongerContainsThisElement()
    {
        MemoryRepositoryImplementation repository = await PrepareRepositoryAsync();

        await repository.DeleteAsync(1);
        
        Assert.False(await repository.ExistAsync(1));
    }
    
    [Fact]
    public async Task DeleteAsync_ElementNotExist_ThrowResourceNotFoundException()
    {
        MemoryRepositoryImplementation repository = await PrepareRepositoryAsync();

        async Task Actual() => await repository.DeleteAsync(-1);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }
}