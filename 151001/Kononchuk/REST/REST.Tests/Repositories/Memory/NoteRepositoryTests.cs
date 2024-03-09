using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;

namespace REST.Tests.Repositories.Memory;

public class NoteRepositoryTests
{
    private NoteRepository PrepareRepository()
    {
        NoteRepository repository = new NoteRepository();
        Note note = new Note() { Content = "created" };

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
        Note note = new Note() { Content = "created" };

        var addedNote = repository.Add(note);


        Assert.True(addedNote?.Id == 1);
    }

    [Fact]
    public void Exist_NotExist_ReturnFalse()
    {
        NoteRepository repository = new NoteRepository();

        bool isExist = repository.Exist(-1);

        Assert.False(isExist);
    }

    [Fact]
    public void Exist_Exist_ReturnTrue()
    {
        NoteRepository repository = PrepareRepository();

        bool isExist = repository.Exist(1);

        Assert.True(isExist);
    }

    [Fact]
    public void GetById_NotExist_ReturnNull()
    {
        NoteRepository repository = PrepareRepository();

        var note = repository.GetById(-1);

        Assert.Null(note);
    }

    [Fact]
    public void GetById_Exist_ReturnExistingResult()
    {
        NoteRepository repository = PrepareRepository();

        var note = repository.GetById(1);

        Assert.NotNull(note);
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnEmptyList()
    {
        NoteRepository repository = new NoteRepository();

        var allNotes = repository.GetAll();

        Assert.Empty(allNotes);
    }

    [Fact]
    public void GetAll_NonEmptyRepository_ReturnNonEmptyList()
    {
        NoteRepository repository = PrepareRepository();

        var allNotes = repository.GetAll();

        Assert.NotEmpty(allNotes);
    }

    [Fact]
    public void Update_NullArgument_ThrowArgumentNullException()
    {
        NoteRepository repository = new NoteRepository();

        void Actual() => repository.Update(1, null!);

        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_NoteNotExist_ReturnNull()
    {
        NoteRepository repository = PrepareRepository();
        Note note = new Note() { Content = "updated" };

        var updateNote = repository.Update(-1, note);

        Assert.Null(updateNote);
    }

    [Fact]
    public void Update_ValidArguments_ReturnUpdatedNote()
    {
        NoteRepository repository = PrepareRepository();
        Note note = new Note() { Content = "created" };

        var updateNote = repository.Update(1, note);

        Assert.Equal(note.Content, updateNote?.Content);
    }

    [Fact]
    public void Delete_NoteExist_ReturnTrue()
    {
        NoteRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(1);

        Assert.True(isDeleted);
    }

    [Fact]
    public void Delete_NoteNotExist_ReturnFalse()
    {
        NoteRepository repository = PrepareRepository();

        bool isDeleted = repository.Delete(-1);

        Assert.False(isDeleted);
    }
}