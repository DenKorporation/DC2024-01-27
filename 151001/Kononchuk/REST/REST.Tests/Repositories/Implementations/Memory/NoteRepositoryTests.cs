using JetBrains.Annotations;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Utilities.Exceptions;

namespace REST.Tests.Repositories.Implementations.Memory;

[TestSubject(typeof(NoteRepository))]
public class NoteRepositoryTests
{
    private NoteRepository PrepareRepository()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note { Content = "created" };

        repository.Add(note);

        return repository;
    }

    [Fact]
    public void Add_NullArgument_ThrowArgumentNullException()
    {
        NoteRepository repository = new NoteRepository();

        void Actual() => repository.Add(null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Add_ValidNote_ReturnNoteWithSetId()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note { Content = "created" };

        var addedNote = repository.Add(note);
        
        Assert.Equal(1, addedNote.Id);
        Assert.Equal(note.Content, addedNote.Content);
    }

    [Fact]
    public void Update_NoteNotExist_ThrowResourceNotFoundException()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note { Content = "updated" };
        
        Note Actual() => repository.Update(-1, note);

        Assert.Throws<ResourceNotFoundException>(Actual);
    }
    
    [Fact]
    public void Update_ValidArguments_ReturnUpdatedNote()
    {
        NoteRepository repository = PrepareRepository();
        Note note = new Note { Content = "updated" };
        
        var updateNote = repository.Update(1, note);

        Assert.Equal(note.Content, updateNote.Content);
    }
}