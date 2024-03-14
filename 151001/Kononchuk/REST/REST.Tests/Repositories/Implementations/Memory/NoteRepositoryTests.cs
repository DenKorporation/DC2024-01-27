using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

[TestSubject(typeof(NoteRepository))]
public class NoteRepositoryTests
{
    private async Task<NoteRepository> PrepareRepositoryAsync()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note { Content = "created" };

        await repository.AddAsync(note);

        return repository;
    }

    [Fact]
    public async Task AddAsync_NullArgument_ThrowArgumentNullException()
    {
        NoteRepository repository = new NoteRepository();

        async Task Actual() => await repository.AddAsync(null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task AddAsync_ValidNote_ReturnNoteWithSetId()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note { Content = "created" };

        var addedNote = await repository.AddAsync(note);
        
        Assert.Equal(1, addedNote.Id);
        Assert.Equal(note.Content, addedNote.Content);
    }

    [Fact]
    public async Task UpdateAsync_NullArgument_ThrowArgumentNullException()
    {
        NoteRepository repository = new NoteRepository();

        async Task Actual() => await repository.UpdateAsync(1, null!);

        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }
    
    [Fact]
    public async Task UpdateAsync_NoteNotExist_ThrowResourceNotFoundException()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note { Content = "updated" };

        async Task<Note> Actual() => await repository.UpdateAsync(-1, note);

        await Assert.ThrowsAsync<ResourceNotFoundException>(Actual);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidArguments_ReturnUpdatedNote()
    {
        NoteRepository repository = await PrepareRepositoryAsync();
        Note note = new Note { Content = "updated" };
        
        var updateNote = await repository.UpdateAsync(1, note);

        Assert.Equal(note.Content, updateNote.Content);
    }
}