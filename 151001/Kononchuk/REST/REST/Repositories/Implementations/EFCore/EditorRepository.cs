using Microsoft.EntityFrameworkCore;
using REST.Data;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;
using UniqueConstraintException = EntityFramework.Exceptions.Common.UniqueConstraintException;

namespace REST.Repositories.Implementations.EFCore;

public class EditorRepository(AppDbContext dbContext) : IEditorRepository<long>
{
    public async Task<Editor> AddAsync(Editor entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await dbContext.Editors.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        catch (UniqueConstraintException)
        {
            throw new Utilities.Exceptions.UniqueConstraintException($"Login {entity.Login} already exist", 40301);
        }

        return entity;
    }

    public async Task<bool> ExistAsync(long id)
    {
        return await dbContext.Editors.FirstOrDefaultAsync(editor => editor.Id == id) is not null;
    }

    public async Task<Editor> GetByIdAsync(long id)
    {
        Editor? result = await dbContext.Editors.FirstOrDefaultAsync(editor => editor.Id == id);
        if (result is null)
        {
            throw new ResourceNotFoundException(code: 40401);
        }

        return result;
    }

    public async Task<IEnumerable<Editor>> GetAllAsync()
    {
        return await dbContext.Editors.ToListAsync();
    }

    public async Task<Editor> UpdateAsync(long id, Editor entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            //TODO maybe I should add some check for the presence of a updated value.
            entity.Id = id;
            dbContext.Editors.Update(entity);
            if (await dbContext.SaveChangesAsync() == 0)
            {
                throw new ResourceNotFoundException(code: 40402);
            }
        }
        catch (UniqueConstraintException)
        {
            throw new Utilities.Exceptions.UniqueConstraintException($"Login {entity.Login} already exist", 40302);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFoundException(code: 40402);
        }

        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        Editor? editor = await dbContext.Editors.FirstOrDefaultAsync(e => e.Id == id);
        if (editor is null)
        {
            throw new ResourceNotFoundException(code: 40403);
        }

        try
        {
            dbContext.Editors.Remove(editor);
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