using REST.Tests.Repositories.Implementations.Memory.Utiles;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

public class MemoryRepositoryTest
{
    private MemoryRepositoryImplementation PrepareRepository()
    {
        MemoryRepositoryImplementation repository = new MemoryRepositoryImplementation();
        string element = "test";

        repository.Add(element);

        return repository;
    }

    [Fact]
    public void Exist_NotExist_ReturnFalse()
    {
        MemoryRepositoryImplementation repository = new MemoryRepositoryImplementation();

        bool isExist = repository.Exist(-1);

        Assert.False(isExist);
    }

    [Fact]
    public void Exist_Exist_ReturnTrue()
    {
        MemoryRepositoryImplementation repository = PrepareRepository();

        bool isExist = repository.Exist(1);

        Assert.True(isExist);
    }
    
    [Fact]
    public void GetById_NotExist_ThrowResourceNotFoundException()
    {
        MemoryRepositoryImplementation repository = PrepareRepository();

        string Actual() => repository.GetById(-1);

        Assert.Throws<ResourceNotFoundException>((Func<string>)Actual);
    }
    
    [Fact]
    public void GetById_Exist_ReturnExistingResult()
    {
        MemoryRepositoryImplementation repository = PrepareRepository();

        var element = repository.GetById(1);

        Assert.NotNull(element);
    }
    
    [Fact]
    public void GetAll_EmptyRepository_ReturnEmptyList()
    {
        MemoryRepositoryImplementation repository = new MemoryRepositoryImplementation();

        var allElements = repository.GetAll();

        Assert.Empty(allElements);
    }
    
    [Fact]
    public void GetAll_NonEmptyRepository_ReturnNonEmptyList()
    {
        MemoryRepositoryImplementation repository = PrepareRepository();

        var allElements = repository.GetAll();

        Assert.NotEmpty(allElements);
    }
    
    [Fact]
    public void Delete_ElementExist_RepositoryNoLongerContainsThisElement()
    {
        MemoryRepositoryImplementation repository = PrepareRepository();

        repository.Delete(1);
        
        Assert.False(repository.Exist(1));
    }
    
    [Fact]
    public void Delete_ElementNotExist_ThrowResourceNotFoundException()
    {
        MemoryRepositoryImplementation repository = PrepareRepository();

        void Actual() => repository.Delete(-1);

        Assert.Throws<ResourceNotFoundException>(Actual);
    }
}