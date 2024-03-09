using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;

namespace REST.Tests.Repositories.Memory;

public class EditorRepositoryTests
{
    private EditorRepository PrepareRepository()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor() { FirstName = "test", LastName = "test", Login = "test", Password = "12345678" };

        repository.Add(editor);

        return repository;
    }
    
    [Fact]
    public void Add_NullArgument_ThrowArgumentNullException()
    {
        EditorRepository repository = new EditorRepository();

        void Actual() => repository.Add(null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Add_ValidEditor_ReturnEditorWithSetId()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor() { FirstName = "test", LastName = "test", Login = "test", Password = "12345678" };

        var addedEditor = repository.Add(editor);


        Assert.True(addedEditor?.Id == 1);
    }

    [Fact]
    public void Exist_NotExist_ReturnFalse()
    {
        EditorRepository repository = new EditorRepository();

        bool isExist = repository.Exist(-1);

        Assert.False(isExist);
    }

    [Fact]
    public void Exist_Exist_ReturnTrue()
    {
        EditorRepository repository = PrepareRepository();

        bool isExist = repository.Exist(1);

        Assert.True(isExist);
    }
    
    [Fact]
    public void GetById_NotExist_ReturnNull()
    {
        EditorRepository repository = PrepareRepository();

        var editor = repository.GetById(-1);

        Assert.Null(editor);
    }
    
    [Fact]
    public void GetById_Exist_ReturnExistingResult()
    {
        EditorRepository repository = PrepareRepository();

        var editor = repository.GetById(1);

        Assert.NotNull(editor);
    }
    
    [Fact]
    public void GetAll_EmptyRepository_ReturnEmptyList()
    {
        EditorRepository repository = new EditorRepository();

        var allEditors = repository.GetAll();

        Assert.Empty(allEditors);
    }
    
    [Fact]
    public void GetAll_NonEmptyRepository_ReturnNonEmptyList()
    {
        EditorRepository repository = PrepareRepository();

        var allEditors = repository.GetAll();

        Assert.NotEmpty(allEditors);
    }
    
    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        EditorRepository repository = new EditorRepository();

        void Actual() => repository.Update(1,null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }
    
    [Fact]
    public void Update_EditorNotExist_ReturnNull()
    {
        EditorRepository repository = PrepareRepository();
        Editor editor = new Editor() { FirstName = "updated", LastName = "test", Login = "test", Password = "12345678" };

        var updateEditor = repository.Update(-1, editor);

        Assert.Null(updateEditor);
    }
    
    [Fact]
    public void Update_ValidArguments_ReturnUpdatedEditor()
    {
        EditorRepository repository = PrepareRepository();
        Editor editor = new Editor() { FirstName = "updated", LastName = "test", Login = "test", Password = "12345678" };

        var updateEditor = repository.Update(1, editor);

        Assert.Equal(editor.FirstName, updateEditor?.FirstName);
    }
    
    [Fact]
    public void Delete_EditorExist_ReturnTrue()
    {
        EditorRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(1);

        Assert.True(isDeleted);
    }
    
    [Fact]
    public void Delete_EditorNotExist_ReturnFalse()
    {
        EditorRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(-1);

        Assert.False(isDeleted);
    }
}