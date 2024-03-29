using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using REST.Data;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.EFCore;

public class NoteRepository(AppDbContext dbContext) : INoteRepository<long>
{
    /// <inheritdoc/>
    /// <exception cref="AssociationException">Occurs if <see cref="Issue"/> with given id not exist</exception>
    public async Task<Note> AddAsync(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await dbContext.Notes.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        catch (ReferenceConstraintException)
        {
            throw new AssociationException($"Issue with Id = {entity.IssueId} not exist", 40311);
        }

        return entity;
    }

    public async Task<bool> ExistAsync(long id)
    {
        return await dbContext.Notes.FirstOrDefaultAsync(note => note.Id == id) is not null;
    }

    public async Task<Note> GetByIdAsync(long id)
    {
        Note? result = await dbContext.Notes.FirstOrDefaultAsync(note => note.Id == id);
        if (result is null)
        {
            throw new ResourceNotFoundException(code: 40401);
        }

        return result;
    }

    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        return await dbContext.Notes.ToListAsync();
    }

    /// <inheritdoc/>
    /// <exception cref="AssociationException">Occurs if <see cref="Issue"/> with given id not exist</exception>
    public async Task<Note> UpdateAsync(long id, Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            entity.Id = id;
            dbContext.Notes.Update(entity);
            if (await dbContext.SaveChangesAsync() == 0)
            {
                throw new ResourceNotFoundException(code: 40402);
            }
        }
        catch (ReferenceConstraintException)
        {
            throw new AssociationException($"Issue with Id = {entity.IssueId} not exist", 40312);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFoundException(code: 40402);
        }

        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        Note? note = await dbContext.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note is null)
        {
            throw new ResourceNotFoundException(code: 40403);
        }

        try
        {
            dbContext.Notes.Remove(note);
            if (await dbContext.SaveChangesAsync() == 0)
            {
                throw new ResourceNotFoundException(code: 40403);
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFoundException(code: 40403);
        }
    }
}