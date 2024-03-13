using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

[TestSubject(typeof(EditorRepository))]
public class EditorRepositoryTests
{
    private EditorRepository PrepareRepository()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor { FirstName = "test", LastName = "test", Login = "test", Password = "12345678" };

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
        Editor editor = new Editor { FirstName = "test", LastName = "test", Login = "test", Password = "12345678" };

        var addedEditor = repository.Add(editor);

        Assert.Equal(1, addedEditor.Id);
        Assert.Equal(editor.FirstName, addedEditor.FirstName);
        Assert.Equal(editor.LastName, addedEditor.LastName);
        Assert.Equal(editor.Login, addedEditor.Login);
        Assert.Equal(editor.Password, addedEditor.Password);
    }
    
    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        EditorRepository repository = new EditorRepository();

        void Actual() => repository.Update(1,null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }
    
    [Fact]
    public void Update_EditorNotExist_ThrowResourceNotFoundException()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor { FirstName = "updated", LastName = "updated", Login = "updated", Password = "87654321" };

        Editor Actual() => repository.Update(-1, editor);

        Assert.Throws<ResourceNotFoundException>(Actual);
    }
    
    [Fact]
    public void Update_ValidArguments_ReturnUpdatedEditor()
    {
        EditorRepository repository = PrepareRepository();
        Editor editor = new Editor { FirstName = "updated", LastName = "updated", Login = "updated", Password = "87654321" };

        var updateEditor = repository.Update(1, editor);

        Assert.Equal(editor.FirstName, updateEditor.FirstName);
        Assert.Equal(editor.LastName, updateEditor.LastName);
        Assert.Equal(editor.Login, updateEditor.Login);
        Assert.Equal(editor.Password, updateEditor.Password);
    }
}