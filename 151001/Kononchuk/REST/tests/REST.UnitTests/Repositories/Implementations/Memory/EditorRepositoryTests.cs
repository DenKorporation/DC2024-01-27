using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.UnitTests.Repositories.Implementations.Memory;

[TestSubject(typeof(EditorRepository))]
public class EditorRepositoryTests
{
    private async Task<EditorRepository> PrepareRepositoryAsync()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor { FirstName = "test", LastName = "test", Login = "test", Password = "12345678" };

        await repository.AddAsync(editor);

        return repository;
    }
    
    [Fact]
    public async Task AddAsync_NullArgument_ThrowsArgumentNullException()
    {
        EditorRepository repository = new EditorRepository();

        async Task Actual() => await repository.AddAsync(null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task AddAsync_ValidEditor_ReturnsEditorWithSetId()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor { FirstName = "test", LastName = "test", Login = "test", Password = "12345678" };

        var addedEditor = await repository.AddAsync(editor);

        Assert.Equal(1, addedEditor.Id);
        Assert.Equal(editor.FirstName, addedEditor.FirstName);
        Assert.Equal(editor.LastName, addedEditor.LastName);
        Assert.Equal(editor.Login, addedEditor.Login);
        Assert.Equal(editor.Password, addedEditor.Password);
    }
    
    [Fact]
    public async Task UpdateAsync_NullArgument_ThrowsArgumentNullException()
    {
        EditorRepository repository = new EditorRepository();

        async Task Actual() => await repository.UpdateAsync(1,null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }
    
    [Fact]
    public async Task UpdateAsync_EditorNotExist_ThrowsResourceNotFoundException()
    {
        EditorRepository repository = new EditorRepository();
        Editor editor = new Editor { FirstName = "updated", LastName = "updated", Login = "updated", Password = "87654321" };

        async Task<Editor> Actual() => await repository.UpdateAsync(-1, editor);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidArguments_ReturnsUpdatedEditor()
    {
        EditorRepository repository = await PrepareRepositoryAsync();
        Editor editor = new Editor { FirstName = "updated", LastName = "updated", Login = "updated", Password = "87654321" };

        var updateEditor = await repository.UpdateAsync(1, editor);

        Assert.Equal(editor.FirstName, updateEditor.FirstName);
        Assert.Equal(editor.LastName, updateEditor.LastName);
        Assert.Equal(editor.Login, updateEditor.Login);
        Assert.Equal(editor.Password, updateEditor.Password);
    }
}